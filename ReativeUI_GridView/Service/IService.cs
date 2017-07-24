using System.Collections.Generic;

namespace ReativeUI_GridView.Service
{
    public interface IService<T>
    { 
       IEnumerable<T> Get();
    }
}