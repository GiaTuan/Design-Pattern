using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MyORM
{
    public abstract class IQueryBuilder
    {
        protected string queryString = null;
        public abstract void Add<T>(T obj) where T : new();
        public abstract void Select<T>(string selectedValues = null);
        public abstract void From<T>();
        public abstract void Where<T>(Expression<Func<T, bool>> func);
        public abstract void GroupBy(string strGroupBy);
        public abstract void Having<T>(Expression<Func<T, bool>> func);
        public abstract void Update<T>(T obj) where T : new();
        public abstract void Delete<T>();

        public string GetQueryString()
        {
            return queryString;
        }
    }
}