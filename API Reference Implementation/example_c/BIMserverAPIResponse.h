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

#include "BIMserverAPIResponse.hxx"

#ifdef  __cplusplus
extern "C" {
#endif

BIMserverAPIResponse *BIMserverAPIResponse_new_true_response(void);

BIMserverAPIResponse *BIMserverAPIResponse_new_true_response_with_response_text(wchar_t **response_text);

BIMserverAPIResponse *BIMserverAPIResponse_new_false_response(void);

BIMserverAPIResponse *BIMserverAPIResponse_new_error_response(wchar_t **error_message);

BIMserverAPIResponse *BIMserverAPIResponse_new_blocked_resource_response(wchar_t **error_message);

void BIMserverAPIResponse_free(BIMserverAPIResponse **response);


#ifdef  __cplusplus
}
#endif
