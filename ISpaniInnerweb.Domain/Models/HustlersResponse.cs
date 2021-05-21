using System.Collections.Generic;

namespace ISpaniInnerweb.Domain.Entities
{
    public class HustlersResponse<T> where T:class
    {
        public string Status { get; set; }
        public string Messages { get; set; }
        public T Data { set;get; }
    }
}
