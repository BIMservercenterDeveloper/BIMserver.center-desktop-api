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

#include "BIMserverAPIResponse.h"

#include <assert.h>
#include <stdlib.h>
#include <wchar.h>

// ----------------------------------------------------------------------------------------------------

static wchar_t *i_dereference_pp_wchar(wchar_t **response_result)
{
    wchar_t *response_result_contents;

    assert(response_result != NULL && *response_result != NULL);

    response_result_contents = *response_result;
    *response_result = NULL;

    return response_result_contents;
}

// ----------------------------------------------------------------------------------------------------

static BIMserverAPIResponse *i_new_response(enum BIMserverAPIResponseType type, wchar_t **response_result)
{
    BIMserverAPIResponse *response;

    assert(response_result != NULL && *response_result != NULL);

    response = (BIMserverAPIResponse *)malloc(sizeof(BIMserverAPIResponse));

    response->type = type;
    response->response_result = i_dereference_pp_wchar(response_result);

    return response;
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIResponse_new_true_response(void)
{
    wchar_t *response_result;

    response_result = _wcsdup(L"");
    return i_new_response(BIMserverAPIResponseType_TRUE, &response_result);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIResponse_new_true_response_with_response_text(wchar_t **response_text)
{
    return i_new_response(BIMserverAPIResponseType_TRUE, response_text);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIResponse_new_false_response(void)
{
    wchar_t *response_result;

    response_result = _wcsdup(L"");
    return i_new_response(BIMserverAPIResponseType_FALSE, &response_result);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIResponse_new_error_response(wchar_t **error_message)
{
    return i_new_response(BIMserverAPIResponseType_ERROR, error_message);
}

// ----------------------------------------------------------------------------------------------------

BIMserverAPIResponse *BIMserverAPIResponse_new_blocked_resource_response(wchar_t **error_message)
{
    return i_new_response(BIMserverAPIResponseType_BLOCKED_RESOURCE, error_message);
}

// ----------------------------------------------------------------------------------------------------

void BIMserverAPIResponse_free(BIMserverAPIResponse **response)
{
    assert(response != NULL && *response != NULL);

    free((*response)->response_result);
    free(*response);

    *response = NULL;
}
