using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIPA.Classes.InputFile {
    partial class InputFile
    {

        /// <summary>
        /// Cellbuilder which handles the cellcreation
        /// </summary>
        public bool CellBuilder()
        {
            bool ok = false;
            CreateCells();
            if (PopulateCells())
                ok = true;

            CalculateMinutes();
            return ok;
        }

    }
}
