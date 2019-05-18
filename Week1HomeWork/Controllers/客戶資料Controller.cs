using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Week1HomeWork.Models;
using ClosedXML.Excel;
using System.IO;

namespace Week1HomeWork.Controllers
{
    public class 客戶資料Controller : Controller
    {
        private 客戶資料Entities db = new 客戶資料Entities();
        客戶資料Repository repo;
        客戶分類Repository repoCatego;

        public 客戶資料Controller()
        {
            repo = RepositoryHelper.Get客戶資料Repository();
            repoCatego = RepositoryHelper.Get客戶分類Repository();
        }


        public ActionResult 客戶關聯資料表()
        {
            return View(db.vw客戶關聯資料統計表.ToList());
        }

        public ActionResult Download(string keyword, string catego)
        {
            //ClosedXML的用法 先new一個Excel Workbook
            using (XLWorkbook wb = new XLWorkbook())
            {
                //取得我要塞入Excel內的資料
                var data = repo.GetList(keyword, catego).Select(c => new {
                    c.Id,
                    c.客戶名稱,
                    c.統一編號,
                    c.電話,
                    c.傳真,
                    c.地址,
                    c.Email,
                    c.客戶分類
                });

                //一個wrokbook內至少會有一個worksheet,並將資料Insert至這個位於A1這個位置上
                var ws = wb.Worksheets.Add("sheet1", 1);
                //注意官方文件上說明,如果是要塞入Query後的資料該資料一定要變成是data.AsEnumerable()
                //但是我查詢出來的資料剛好是IQueryable ,其中IQueryable有繼承IEnumerable 所以不需要特別寫
                ws.Cell(1, 1).InsertData(data);

                //因為是用Query的方式,這個地方要用串流的方式來存檔
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    //請注意 一定要加入這行,不然Excel會是空檔
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    //注意Excel的ContentType,是要用這個"application/vnd.ms-excel" 不曉得為什麼網路上有的Excel ContentType超長,xlsx會錯 xls反而不會
                    return this.File(memoryStream.ToArray(), "application/vnd.ms-excel", "Download.xlsx");
                }
            }

        }



        // GET: 客戶資料
        public ActionResult Index(string keyword,string catego)
        {
            var data = repo.GetList(keyword,catego);

            /*
            var data = db.客戶資料.Where(p => false == p.是否已刪除).AsQueryable();
            if (!String.IsNullOrEmpty(keyword))
            {
                data = data.Where(p => p.客戶名稱.Contains(keyword));
            }
            */

            var selectItemList = new List<SelectListItem>() {
                new SelectListItem(){Value="",Text="全部",Selected=true}
            };
            var selectList = new SelectList(repoCatego.All(), "分類ID", "客戶分類名稱");
            selectItemList.AddRange(selectList);

            ViewBag.客戶分類 = selectItemList;
            ViewBag.keyword = keyword;
            ViewBag.catego = catego;
            return View(data.ToList());
        }

        // GET: 客戶資料/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: 客戶資料/Create
        public ActionResult Create()
        {
            ViewBag.客戶分類 = new SelectList(repoCatego.All(), "分類ID", "客戶分類名稱");
            return View();
        }

        // POST: 客戶資料/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.Add(客戶資料);
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶分類 = new SelectList(repoCatego.All(), "分類ID", "客戶分類名稱");
            return View(客戶資料);
        }

        // GET: 客戶資料/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = db.客戶資料.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }

            ViewBag.客戶分類 = new SelectList(repoCatego.All(), "分類ID", "客戶分類名稱" , 客戶資料.客戶分類);

            return View(客戶資料);
        }

        // POST: 客戶資料/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶名稱,統一編號,電話,傳真,地址,Email,是否已刪除,客戶分類")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                repo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                repo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶分類 = new SelectList(repoCatego.All(), "分類ID", "客戶分類名稱", 客戶資料.客戶分類);
            return View(客戶資料);
        }

        // GET: 客戶資料/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = repo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: 客戶資料/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = repo.Find(id);
            repo.Delete(客戶資料);
            repo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
