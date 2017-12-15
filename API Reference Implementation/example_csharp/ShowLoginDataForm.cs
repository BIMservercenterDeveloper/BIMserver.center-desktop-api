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
using BIMserverAPI;

namespace example_csharp
{
    public partial class ShowLoginDataForm : Form
    {
        public ShowLoginDataForm()
        {
            InitializeComponent();
        }

        public void populateData()
        {
            BIMserverAPIResponse userNameResponse = BIMserverAPIWrapper.getLoggedUserName();
            Debug.Assert(userNameResponse.type == BIMserverAPIResponseType.TRUE || userNameResponse.type == BIMserverAPIResponseType.ERROR);
            userName.Text = "User name: " + userNameResponse.responseText;

            BIMserverAPIResponse userEmailResponse = BIMserverAPIWrapper.getLoggedUserEmail();
            Debug.Assert(userEmailResponse.type == BIMserverAPIResponseType.TRUE || userEmailResponse.type == BIMserverAPIResponseType.ERROR);
            userEmail.Text = "Email: " + userEmailResponse.responseText;

            BIMserverAPIResponse userImageResponse = BIMserverAPIWrapper.getLoggedUserImage();
            Debug.Assert(userImageResponse.type == BIMserverAPIResponseType.TRUE || userImageResponse.type == BIMserverAPIResponseType.ERROR);

            if (userImageResponse.type == BIMserverAPIResponseType.TRUE)
            {
                userImage.ImageLocation = userImageResponse.responseText;
                userImage.Load();
            }
            else
            {
                Debug.Assert(userEmailResponse.type == BIMserverAPIResponseType.ERROR);
            }
        }
    }
}
