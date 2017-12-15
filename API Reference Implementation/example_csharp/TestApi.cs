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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using BIMserverAPI;

namespace example_csharp
{
    public partial class TestApi : Form
    {
        string selectedIfcFileFromCurrentProject;
        string selectedAssociatedFileFromCurrentProject;

        public TestApi()
        {
            InitializeComponent();

            selectedIfcFileFromCurrentProject = null;
            selectedAssociatedFileFromCurrentProject = null;
        }

        private void createDBBtn_Click(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.createDatabase();

            if (response.type == BIMserverAPIResponseType.TRUE)
            {
                MessageBox.Show("BIMserverAPI Database created");
            }
            else
            {
                Debug.Assert(response.type == BIMserverAPIResponseType.ERROR);
                MessageBox.Show(response.responseText);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.loginForm();

            switch (response.type)
            {
                case BIMserverAPIResponseType.TRUE:             MessageBox.Show("User is logged");      break;
                case BIMserverAPIResponseType.FALSE:            MessageBox.Show("User is not logged");  break;
                case BIMserverAPIResponseType.BLOCKED_RESOURCE: MessageBox.Show(response.responseText); break;
                case BIMserverAPIResponseType.ERROR:            MessageBox.Show(response.responseText); break;
            }
        }

        private void chkLoginBtn_Click_1(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.isLogged();
            Debug.Assert(response.type != BIMserverAPIResponseType.BLOCKED_RESOURCE);

            switch (response.type)
            {
                case BIMserverAPIResponseType.TRUE: MessageBox.Show("User is logged"); break;
                case BIMserverAPIResponseType.FALSE: MessageBox.Show("User is not logged"); break;
                case BIMserverAPIResponseType.ERROR: MessageBox.Show(response.responseText); break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.selectCurrentProject();
            
            switch (response.type)
            {
                case BIMserverAPIResponseType.TRUE:                 MessageBox.Show(response.responseText); break;
                case BIMserverAPIResponseType.FALSE:                MessageBox.Show("No project selected or changed"); break;
                case BIMserverAPIResponseType.BLOCKED_RESOURCE:     MessageBox.Show(response.responseText); break;
                case BIMserverAPIResponseType.ERROR:                MessageBox.Show(response.responseText); break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.selectIfcFileFromCurrectProject();
            Debug.Assert(response.type != BIMserverAPIResponseType.BLOCKED_RESOURCE);

            switch (response.type)
            {
                case BIMserverAPIResponseType.TRUE:
                
                    MessageBox.Show(response.responseText);
                    selectedIfcFileFromCurrentProject = response.responseText;
                    break;

                case BIMserverAPIResponseType.FALSE:

                    MessageBox.Show("No file selected");
                    break;

                case BIMserverAPIResponseType.ERROR:

                    MessageBox.Show(response.responseText);
                    break;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.selectAssociatedFileFromCurrectProject("txt;docx;pdf;dwg;dxf");
            Debug.Assert(response.type != BIMserverAPIResponseType.BLOCKED_RESOURCE);

            switch (response.type)
            {
                case BIMserverAPIResponseType.TRUE:

                    MessageBox.Show(response.responseText);
                    selectedAssociatedFileFromCurrentProject = response.responseText;
                    break;

                case BIMserverAPIResponseType.FALSE:

                    MessageBox.Show("No file selected");
                    break;

                case BIMserverAPIResponseType.ERROR:

                    MessageBox.Show(response.responseText);
                    break;
            }
        }

        static private string convertRelativeProjectFilePathToAbsolute(string projectRelativeFilePath, bool updateDatabaseTimeStamp)
        {
            if (projectRelativeFilePath == null)
            {
                return "No IFC file was selected";
            }
            else
            {
                BIMserverAPIResponse response = BIMserverAPIWrapper.convertRelativeProjectFilePathToAbsolute(projectRelativeFilePath, updateDatabaseTimeStamp);
                Debug.Assert(response.type == BIMserverAPIResponseType.TRUE || response.type == BIMserverAPIResponseType.ERROR);

                return response.responseText;
            }
        }

        private void getAbsFilePathBtn_Click(object sender, EventArgs e)
        {
            bool updateDatabaseTimeStamp;
            string absoluteIfcFilePath, absoluteFilePathAssociatedFile;
            string absolutePaths;

            updateDatabaseTimeStamp = true;
            absoluteIfcFilePath = convertRelativeProjectFilePathToAbsolute(selectedIfcFileFromCurrentProject, updateDatabaseTimeStamp);
            absoluteFilePathAssociatedFile = convertRelativeProjectFilePathToAbsolute(selectedAssociatedFileFromCurrentProject, updateDatabaseTimeStamp);

            absolutePaths  = "IFC file: " + absoluteIfcFilePath + "\n";
            absolutePaths += "Associated file: " + absoluteFilePathAssociatedFile;

            MessageBox.Show(absolutePaths);
        }

        private void putIFCBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();

            opd.Filter = "IFC Files (*.ifc) | *.ifc";
            opd.FilterIndex = 1;
            opd.Multiselect = false;

            opd.CheckFileExists = true;
            opd.CheckPathExists = true;

            DialogResult selected = opd.ShowDialog();

            if (selected == DialogResult.OK)
            {
                BIMserverAPIResponse response = BIMserverAPIWrapper.putFileInCurrentProject(opd.FileName);

                if (response.type == BIMserverAPIResponseType.TRUE)
                {
                    MessageBox.Show("File added correctly");
                }
                else
                {
                    Debug.Assert(response.type == BIMserverAPIResponseType.ERROR);
                    MessageBox.Show(response.responseText);
                }
            }
        }

        private void putAssociatedBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog opd = new OpenFileDialog();

            opd.Filter = "Text files (*.txt) | *.txt | PDF files (*.pdf) | *.pdf";
            opd.FilterIndex = 1;
            opd.Multiselect = false;

            opd.CheckFileExists = true;
            opd.CheckPathExists = true;

            DialogResult selected = opd.ShowDialog();

            if (selected == DialogResult.OK)
            {
                BIMserverAPIResponse response = BIMserverAPIWrapper.putAssociatedFileInCurrentProject(
                        opd.FileName, 
                        "An associated file",
                        "BIMserver.center demo test api", "0.1", "API Demo tools");

                if (response.type == BIMserverAPIResponseType.TRUE)
                {
                    MessageBox.Show("File added correctly");
                }
                else
                {
                    Debug.Assert(response.type == BIMserverAPIResponseType.ERROR);
                    MessageBox.Show(response.responseText);
                }
            }

        }

        private void getCurrenProjectPathBtn_Click(object sender, EventArgs e)
        {
            BIMserverAPIResponse response = BIMserverAPIWrapper.getAbsoluteCurrentProjectPath();

            if (response.type == BIMserverAPIResponseType.TRUE)
            {
                MessageBox.Show(response.responseText);
            }
            else
            {
                Debug.Assert(response.type == BIMserverAPIResponseType.ERROR);
                MessageBox.Show(response.responseText);
            }
        }

        private static string existsUpdatedVersionOfFile(string relativeFilePathToProject)
        {
            if (relativeFilePathToProject == null)
            {
                return "No file selected";
            }
            else
            {
                BIMserverAPIResponse response = BIMserverAPIWrapper.existsUpdatedFileVersion(relativeFilePathToProject);
                Debug.Assert(response.type != BIMserverAPIResponseType.BLOCKED_RESOURCE);

                switch (response.type)
                {
                    case BIMserverAPIResponseType.TRUE:

                        return "YES";

                    case BIMserverAPIResponseType.FALSE:

                        return "NO";

                    default:

                        Debug.Assert(response.type == BIMserverAPIResponseType.ERROR);
                        return response.responseText;                   
                }
            }
        }

        private void checkUpdatedFile_Click(object sender, EventArgs e)
        {
            string updateIfcFileStatus, updateAssociatedFileStatus;
            string status;

            updateIfcFileStatus = existsUpdatedVersionOfFile(selectedIfcFileFromCurrentProject);
            updateAssociatedFileStatus = existsUpdatedVersionOfFile(selectedAssociatedFileFromCurrentProject);

            status = "IFC file: " + updateIfcFileStatus + "\n";
            status += "Associated file: " + updateAssociatedFileStatus;

            MessageBox.Show(status);
        }

        private void showLoginData_Click(object sender, EventArgs e)
        {
            ShowLoginDataForm form = new ShowLoginDataForm();

            form.populateData();
            form.ShowDialog();

            form.Close();
        }
    }
}
