/*
BIMserver.center license

This file is part of BIMserver.center IFC frameworks.
Copyright (c) 2017 BIMserver.center

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files, to use this software with the
purpose of developing new tools for the BIMserver.center platform or interacting
with it.

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using BIMserverAPI;

namespace example_csharp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (BIMserverAPIWrapper.isApiInstalled() == false)
            {
                MessageBox.Show("API not found");
            }
            else
            {
                //BIMserverAPIWrapper.setDebugMode(true);
                BIMserverAPIWrapper.setLanguage(BIMserverAPILanguage.ENGLISH);
                BIMserverAPIWrapper.setDatabasePath("c:\\BIMserver.center api demo");
                BIMserverAPIWrapper.setLoginPath("c:\\BIMserver.center api demo");

                // This values must be set. You'll get them after registration.
                //BIMserverAPIWrapper.setAppID("app_undefined_id");
                //BIMserverAPIWrapper.setDeveloperID("app_developer_id");

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new TestApi());
            }
        }
    }
}
