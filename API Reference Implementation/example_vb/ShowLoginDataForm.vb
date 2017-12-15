'BIMserver.center license
'
'This file is part of BIMserver.center IFC frameworks.
'Copyright (c) 2017 BIMserver.center

'Permission is hereby granted, free of charge, to any person obtaining a copy of
'this software and associated documentation files, to use this software with the
'purpose of developing new tools for the BIMserver.center platform or interacting
'with it.
'
'The above copyright notice and this permission notice shall be included in all
'copies or substantial portions of the Software.
'
'THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
'IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
'FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
'COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
'IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
'CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

Imports example_vb.BIMserverAPIVB

Public Class ShowLoginDataForm

    Private Sub ShowLoginDataForm_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub

    Public Sub populateData()

        Dim userNameResponse As BIMserverAPIResponse = BIMserverAPIWrapper.getLoggedUserName()
        Debug.Assert(userNameResponse.type = BIMserverAPIResponseType.vbTRUE Or userNameResponse.type = BIMserverAPIResponseType.vbERROR)

        Dim userEmailResponse As BIMserverAPIResponse = BIMserverAPIWrapper.getLoggedUserEmail()
        Debug.Assert(userEmailResponse.type = BIMserverAPIResponseType.vbTRUE Or userEmailResponse.type = BIMserverAPIResponseType.vbERROR)

        Dim userImageResponse As BIMserverAPIResponse = BIMserverAPIWrapper.getLoggedUserImage()
        Debug.Assert(userImageResponse.type = BIMserverAPIResponseType.vbTRUE Or userImageResponse.type = BIMserverAPIResponseType.vbERROR)

        Label1.Text = userNameResponse.responseText
        Label2.Text = userEmailResponse.responseText

        If (userImageResponse.type = BIMserverAPIResponseType.vbTRUE) Then
            PictureBox1.ImageLocation = userImageResponse.responseText
            PictureBox1.Load()
        Else
            Debug.Assert(userEmailResponse.type = BIMserverAPIResponseType.vbERROR)
        End If

    End Sub

End Class