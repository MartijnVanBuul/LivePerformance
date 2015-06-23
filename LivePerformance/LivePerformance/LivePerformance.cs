using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LivePerformance
{
    public partial class LivePerformance : Form
    {
        private Database.Database _database;
        public LivePerformance()
        {
            InitializeComponent();

            _database = new Database.Database();
        }
    }
}
