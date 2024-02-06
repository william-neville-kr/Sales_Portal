using PhysicianPortal.Core.Data;
using PhysicianPortal.Core.Repository;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PhysicianPortal.Core.Helper
{
    public static class SettingManager
    {
        #region Constants
        //
        //protected readonly UnitOfWork _unitOfWork = new UnitOfWork();
        private static readonly UnitOfWork _unitOfWork = new UnitOfWork();
        //private static readonly MH_DWEntities Context = new MH_DWEntities();
        #endregion

        #region Methods

        public static ApplicationSetting GetSettingById(int settingId)
        {
            ApplicationSetting setting = _unitOfWork.ApplicationSettingRepository.GetSingle(t => t.ApplicationSettingId == settingId);
            return setting;
        }

        public static void Delete(int settingId)
        {
            ApplicationSetting setting = _unitOfWork.ApplicationSettingRepository.GetSingle(t => t.ApplicationSettingId == settingId);

            _unitOfWork.ApplicationSettingRepository.Delete(setting);
            _unitOfWork.Save();
        }

        public static IQueryable<ApplicationSetting> GetAllSettings()
        {
            UnitOfWork db = new UnitOfWork();
            IQueryable<ApplicationSetting> col = db.ApplicationSettingRepository.GetAsQuerable();
            return col;
        }

        public static ApplicationSetting SetValue(string name, string value)
        {
            ApplicationSetting setting = _unitOfWork.ApplicationSettingRepository.GetAsQuerable(t => t.Name == name).FirstOrDefault();
            return setting != null ? Update(setting.ApplicationSettingId, name, value) : Add(name, value);
        }

        public static ApplicationSetting Add(string name, string value)
        {
            var setting = new ApplicationSetting { Name = name, Value = value };

            _unitOfWork.ApplicationSettingRepository.Insert(setting);
            _unitOfWork.Save();
            return setting;
        }

        private static ApplicationSetting Update(int settingId, string name, string value)
        {
            IQueryable<ApplicationSetting> settingCollection = GetAllSettings();
            ApplicationSetting item = settingCollection.FirstOrDefault(t => t.ApplicationSettingId == settingId);
            if (item != null)
            {
                item.Value = value;
                item.Name = name;
            }
            else
            {
                item = Add(name, value);
            }
            _unitOfWork.Save();

            return item;
        }

        public static bool GetSettingValueBoolean(string name, bool defaultValue = false)
        {
            string value = GetSettingValue(name);
            if (value.Length > 0)
            {
                return bool.Parse(value);
            }
            return defaultValue;
        }

        public static int GetSettingValueInteger(string name, int defaultValue = 0)
        {
            string value = GetSettingValue(name);
            if (value.Length > 0)
            {
                return int.Parse(value);
            }
            return defaultValue;
        }

        public static decimal GetSettingValueDecimal(string name, decimal defaultValue = 0)
        {
            string value = GetSettingValue(name);
            if (value.Length > 0)
            {
                return decimal.Parse(value, new CultureInfo("en-US"));
            }
            return defaultValue;
        }

        public static string GetSettingValue(string name)
        {
            ApplicationSetting setting = GetSettingByName(name);
            if (setting != null)
                return setting.Value;
            return string.Empty;
        }

        public static ApplicationSetting GetSettingByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return null;

            IQueryable<ApplicationSetting> settingCollection = GetAllSettings();
            return
                settingCollection.FirstOrDefault(
                    setting => setting.Name.ToLower().Equals(name));
        }

        #endregion
    }
}
