using Domain.Archives;
using System;
using System.Collections.Generic;
using System.Web;

namespace Services.Archives
{
    public interface IArchiveService
    {
        int InsertByUrl(List<string> fileInfo, string type, int pid, string memo = null);
        int Insert(HttpPostedFileBase file, string type, int pid, string memo = null);
        Archive GetById(int id);
    }
}
