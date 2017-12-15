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


#if !defined(BIMSERVERAPIRESPONSE_HXX)
#define BIMSERVERAPIRESPONSE_HXX

#ifdef  __cplusplus
extern "C" {
#endif

#include <wchar.h>

enum BIMserverAPIResponseType
{ 
    BIMserverAPIResponseType_TRUE, 
    BIMserverAPIResponseType_FALSE, 
    BIMserverAPIResponseType_BLOCKED_RESOURCE, 
    BIMserverAPIResponseType_ERROR 
};

struct BIMserverAPIResponse_t
{
    enum BIMserverAPIResponseType type;
    wchar_t *response_result;
};

typedef struct BIMserverAPIResponse_t BIMserverAPIResponse;

#ifdef  __cplusplus
}
#endif

#endif