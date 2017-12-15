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

Namespace BIMserverAPIVB

    Public Enum BIMserverAPIResponseType
        vbTRUE
        vbFALSE
        vbBLOCKED_RESOURCE
        vbERROR
    End Enum

    Public Class BIMserverAPIResponse

        Public type As BIMserverAPIResponseType
        Public responseText As String

        Private Sub New(ByVal type As BIMserverAPIResponseType, ByVal responseText As String)
            Me.type = type
            Me.responseText = responseText
        End Sub

        Public Shared Function makeTrueResponse() As BIMserverAPIResponse
            Return New BIMserverAPIResponse(BIMserverAPIResponseType.vbTRUE, "")
        End Function

        Public Shared Function makeTrueResponseWithResponseText(ByVal responseText As String) As BIMserverAPIResponse
            Return New BIMserverAPIResponse(BIMserverAPIResponseType.vbTRUE, responseText)
        End Function

        Public Shared Function makeFalseResponse() As BIMserverAPIResponse
            Return New BIMserverAPIResponse(BIMserverAPIResponseType.vbFALSE, "")
        End Function

        Public Shared Function makeBlockedResourceResponse(ByVal responseText As String) As BIMserverAPIResponse
            Return New BIMserverAPIResponse(BIMserverAPIResponseType.vbBLOCKED_RESOURCE, responseText)
        End Function

        Public Shared Function makeErrorResponse(ByVal responseText As String) As BIMserverAPIResponse
            Return New BIMserverAPIResponse(BIMserverAPIResponseType.vbERROR, responseText)
        End Function

    End Class

End Namespace
