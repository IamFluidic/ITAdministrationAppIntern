using SQLHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;

namespace SQLHelper
{
    public class KeyValue
    {
        public KeyValue() { }
        public KeyValue(string key,object value, DbType dbType = DbType.String, int size = 0) {
            Key = key;
            Value = value;
            DbType = dbType;
            Size = size;


        }

        public string Key { get; set; }
       public object Value { get; set; }
        public DbType DbType { get; set; }
        public int Size { get; set; }
    }
}

