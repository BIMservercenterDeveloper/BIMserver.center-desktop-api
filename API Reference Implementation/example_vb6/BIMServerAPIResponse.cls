VERSION 1.0 CLASS
BEGIN
  MultiUse = -1  'True
  Persistable = 0  'NotPersistable
  DataBindingBehavior = 0  'vbNone
  DataSourceBehavior  = 0  'vbNone
  MTSTransactionMode  = 0  'NotAnMTSObject
END
Attribute VB_Name = "BIMserverAPIResponse"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = True
Attribute VB_PredeclaredId = False
Attribute VB_Exposed = False
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

Option Explicit

Public Enum BIMserverAPIResponseType
        vbTrue
        vbFalse
        vbBLOCKED_RESOURCE
        vbError
End Enum

Private mResponseType As BIMserverAPIResponseType
Private mresponseText As String

Public Property Get ResponseType() As BIMserverAPIResponseType
    ResponseType = mResponseType
End Property

Public Property Let ResponseType(ByVal vResponseType As BIMserverAPIResponseType)
    mResponseType = vResponseType
End Property
Public Property Get responseText() As String
    responseText = mresponseText
End Property

Public Property Let responseText(ByVal vResponseText As String)
    mresponseText = vResponseText
End Property


Private Sub Class_Initialize()
    ResponseType = vbFalse
    responseText = ""
End Sub


Private Sub Class_Terminate()
    mResponseType = vbFalse
    mresponseText = vbNullString
End Sub
