using System;
using Microsoft.EntityFrameworkCore;

namespace Health.Web
{
    public class ApllicationDBContext:DbContext
    {
        public ApllicationDBContext()
        {
			#region Public properties

			//Public DbSet<SettingsDataModel> Settings {get;set;}
            #endregion
        }
    }
}
