using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Collections;
using System.Web.Mvc;

namespace Week1HomeWork.Models
{
    public class 客戶分類Repository : EFRepository<客戶分類>, I客戶類別Repository
    {


    }

    public interface I客戶類別Repository : IRepository<客戶分類>
    {

    }
}