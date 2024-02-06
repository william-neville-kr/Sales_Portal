using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TuesPechkin;

namespace PhysicianPortal.Web.Controllers
{
    public class PDFHelper
    {
        private static ThreadSafeConverter pdfConverter = null;

        private PDFHelper()
        {
            pdfConverter = new ThreadSafeConverter(
                                                new RemotingToolset<PdfToolset>(
                                                    new WinAnyCPUEmbeddedDeployment(
                                                        new TempFolderDeployment()))); 
        }

        public static ThreadSafeConverter SharedInstance
        {
            get
            {
                if (pdfConverter == null)
                {
                    pdfConverter = new ThreadSafeConverter(
                                                new RemotingToolset<PdfToolset>(
                                                    new WinAnyCPUEmbeddedDeployment(
                                                        new TempFolderDeployment()))); 
                }

                return pdfConverter;
            }
        }
    }
}