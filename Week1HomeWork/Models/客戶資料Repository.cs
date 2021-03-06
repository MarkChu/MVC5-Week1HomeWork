using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Collections;
using System.Web.Mvc;

namespace Week1HomeWork.Models
{   
	public  class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
	{

        public IQueryable<客戶資料> GetList(string keyword , string catego) {
            var data = this.All().Where(p => false == p.是否已刪除).AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                data = data.Where(p => p.客戶名稱.Contains(keyword));
            }
            if (!String.IsNullOrEmpty(catego))
            {
                var c = Int32.Parse(catego);
                data = data.Where(p => p.客戶分類 == c);
            }

            return data;
        }


        public 客戶資料 Find(int id)
        {
            return this.All().Where(p => p.Id == id).FirstOrDefault();
        }


        //修正delete
        public override void Delete(客戶資料 entity)
        {
            entity.是否已刪除 = true;
            this.UnitOfWork.Context.Entry(entity).State = EntityState.Modified;
            //this.UnitOfWork.Commit();
        }




    }

	public  interface I客戶資料Repository : IRepository<客戶資料>
	{

	}
}