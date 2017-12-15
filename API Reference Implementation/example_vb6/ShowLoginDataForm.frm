VERSION 5.00
Begin VB.Form Form2 
   Caption         =   "Form2"
   ClientHeight    =   4905
   ClientLeft      =   60
   ClientTop       =   345
   ClientWidth     =   5055
   LinkTopic       =   "Form2"
   ScaleHeight     =   4905
   ScaleWidth      =   5055
   StartUpPosition =   3  'Windows Default
   Begin VB.PictureBox Picture1 
      Height          =   2415
      Left            =   120
      ScaleHeight     =   2355
      ScaleWidth      =   4755
      TabIndex        =   0
      Top             =   120
      Width           =   4815
   End
   Begin VB.Label Label2 
      Caption         =   "Label2"
      Height          =   375
      Left            =   120
      TabIndex        =   2
      Top             =   3240
      Width           =   4815
   End
   Begin VB.Label Label1 
      Caption         =   "Label1"
      Height          =   255
      Left            =   120
      TabIndex        =   1
      Top             =   2760
      Width           =   4815
   End
End
Attribute VB_Name = "Form2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Public Sub populateData()

    Dim userNameResponse As BIMserverAPIResponse
    
    Set userNameResponse = BIMServerAPIWrapper.getLoggedUserName()
    Debug.Assert (userNameResponse.ResponseType = BIMserverAPIResponseType.vbTrue Or userNameResponse.ResponseType = BIMserverAPIResponseType.vbError)

    Dim userEmailResponse As BIMserverAPIResponse
    
    Set userEmailResponse = BIMServerAPIWrapper.getLoggedUserEmail()
    Debug.Assert (userEmailResponse.ResponseType = BIMserverAPIResponseType.vbTrue Or userEmailResponse.ResponseType = BIMserverAPIResponseType.vbError)

    Dim userImageResponse As BIMserverAPIResponse
    
    Set userImageResponse = BIMServerAPIWrapper.getLoggedUserImage()
    Debug.Assert (userImageResponse.ResponseType = BIMserverAPIResponseType.vbTrue Or userImageResponse.ResponseType = BIMserverAPIResponseType.vbError)

    Label1.Caption = userNameResponse.responseText
    Label2.Caption = userEmailResponse.responseText

    If (userImageResponse.ResponseType = BIMserverAPIResponseType.vbTrue) Then
        Picture1.Picture = LoadPicture(userImageResponse.responseText)
    Else
        Debug.Assert (userEmailResponse.ResponseType = BIMserverAPIResponseType.vbError)
    End If

End Sub

