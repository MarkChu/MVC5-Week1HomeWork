using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Week1HomeWork.Models
{   
	public  class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
	{

        public IQueryable<客戶聯絡人> GetList(string keyword)
        {
            var data = this.All().Where(p => false == p.是否已刪除).AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                data = data.Where(p => p.姓名.Contains(keyword));
            }

            return data;
        }


        public 客戶聯絡人 Find(int id)
        {
            return this.All().Where(p => p.Id == id).FirstOrDefault();
        }

        //修正delete
        public override void Delete(客戶聯絡人 entity)
        {
            entity.是否已刪除 = true;
            this.UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
            this.UnitOfWork.Commit();
        }


    }

	public  interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
	{

	}
}