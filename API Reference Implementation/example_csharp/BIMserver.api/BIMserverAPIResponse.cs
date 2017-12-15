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
using System.Text;
using System.Threading.Tasks;

namespace BIMserverAPI
{
    public enum BIMserverAPIResponseType { TRUE, FALSE, BLOCKED_RESOURCE, ERROR }

    public class BIMserverAPIResponse
    {
        public BIMserverAPIResponseType type;
        public string responseText;

        public static BIMserverAPIResponse makeTrueResponse()
        {
            return new BIMserverAPIResponse(BIMserverAPIResponseType.TRUE);
        }

        public static BIMserverAPIResponse makeTrueResponseWithResponseText(string responseText)
        {
            BIMserverAPIResponse response;

            response = new BIMserverAPIResponse(BIMserverAPIResponseType.TRUE);
            response.responseText = responseText;

            return response;
        }

        public static BIMserverAPIResponse makeFalseResponse()
        {
            return new BIMserverAPIResponse(BIMserverAPIResponseType.FALSE);
        }

        public static BIMserverAPIResponse makeErrorResponse(string error_message)
        {
            BIMserverAPIResponse response;

            response = new BIMserverAPIResponse(BIMserverAPIResponseType.ERROR);
            response.responseText = error_message;

            return response;
        }

        public static BIMserverAPIResponse makeBlockedResourceResponse(string text)
        {
            BIMserverAPIResponse response;

            response = new BIMserverAPIResponse(BIMserverAPIResponseType.BLOCKED_RESOURCE);
            response.responseText = text;

            return response;
        }

        private BIMserverAPIResponse(BIMserverAPIResponseType responseType)
        {
            type = responseType;
            responseText = null;
        }
    }
}
