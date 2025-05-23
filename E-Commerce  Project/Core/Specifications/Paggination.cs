﻿

namespace Core.Specifications
{
    public class Paggination <T>
    {

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Paggination(int pageIndex, int pageSize, IReadOnlyList<T> data , int count)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Data = data;
            Count = count;
        }


    }
}
