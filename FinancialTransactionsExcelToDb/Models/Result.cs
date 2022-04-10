using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialTransactionsExcelToDb.Models
{
    public class Result
    {
        public Result()
        {
            Success = true;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class Result<T> : Result
    {
        public T Data { get; set; }
        public dynamic DataInfo { get; set; }
        public Result(T data)
        {
            Data = data;
        }
        public Result()
        {

        }
        public Result(T data, dynamic dataInfo)
        {
            Data = data;
            DataInfo = dataInfo;
        }
    }
}
