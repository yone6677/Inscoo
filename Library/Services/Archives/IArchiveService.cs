﻿using Domain;
using System;
using System.Collections.Generic;
using System.Web;

namespace Services
{
    public interface IArchiveService
    {
        int InsertByUrl(List<string> fileInfo, string type, int pid, string memo = null);
        int Insert(HttpPostedFileBase file, string type, int pid, string memo = null);
        Archive GetById(int id);
    }
}
