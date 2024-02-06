using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using SalesTeam.Core.Data;

namespace SalesTeam.Core.Helpers
{
    public static class SettingManager
    {
        #region Constants
        private static readonly MH_DWEntities Context = new MH_DWEntities();
        #endregion

        #region Methods

        public static ApplicationSetting GetSettingById(int settingId)
        {
            ApplicationSetting setting = Context.ApplicationSettings.Single(t => t.ApplicationSettingId == settingId);
            return setting;
        }

        public static void Delete(int settingId)
        {
            ApplicationSetting setting = Context.ApplicationSettings.Single(t => t.ApplicationSettingId == settingId);

            Context.ApplicationSettings.Remove(setting);
            Context.SaveChanges();
        }

        public static List<ApplicationSetting> GetAllSettings()
        {
            List<ApplicationSetting> col = Context.ApplicationSettings.ToList();
            return col;
        }

        public static ApplicationSetting SetValue(string name, string value)
        {
            ApplicationSetting setting = Context.ApplicationSettings.First(t => t.Name == name);
            return setting != null ? Update(setting.ApplicationSettingId, name, value) : Add(name, value);
        }

        public static ApplicationSetting Add(string name, string value)
        {
            var setting = new ApplicationSetting { Name = name, Value = value };

            Context.ApplicationSettings.Add(setting);
            Context.SaveChanges();
            return setting;
        }

        private static ApplicationSetting Update(int settingId, string name, string value)
        {
            List<ApplicationSetting> settingCollection = GetAllSettings();
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
            Context.SaveChanges();

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

            List<ApplicationSetting> settingCollection = GetAllSettings();
            return
                settingCollection.FirstOrDefault(
                    setting => String.Equals(setting.Name, name, StringComparison.CurrentCultureIgnoreCase));
        }

        #endregion
    }
}
