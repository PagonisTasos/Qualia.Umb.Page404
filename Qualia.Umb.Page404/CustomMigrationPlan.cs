using Qualia.Umb.PackageMigration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Packaging;

namespace Qualia.Umb.Page404
{
    public class Page404MigrationPlan : PackageMigrationPlan
    {
        private static readonly string MIGRATION_PLAN_NAME = "Page404MigrationPlan";
        private static readonly string MIGRATION_STATE_NAME = "initial";

        public Page404MigrationPlan()
            : base(MIGRATION_PLAN_NAME)
        { }

        protected override void DefinePlan()
        {
            To<ImportPackageXmlMigration>(MIGRATION_STATE_NAME);
        }
    }

}
