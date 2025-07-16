using System.Collections.Generic;

namespace MyFinances.Core.Response
{
    public class Response
    {
        public Response() => Errors = [];

        public List<Error> Errors { get; set; }

        public bool IsSuccess => Errors == null || Errors.Count == 0;
    }
}
