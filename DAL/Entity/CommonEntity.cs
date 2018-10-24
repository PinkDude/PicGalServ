using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DAL.Entity
{
    public class CommonEntity
    {
        public int Id { get; set; }

        public virtual IEnumerable<string> GetListOfFields()
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<string> GetFields()
        {
            throw new NotImplementedException();
        }

        public virtual void MapFromDataReader(IDataReader reader)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<string> GetFieldsForSearch()
        {
            throw new NotImplementedException();
        }

        public virtual string GetNameOfTable()
        {
            throw new NotImplementedException();
        }
    }
}
