
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace OptiSolve
{
    public class MainForm : Form
    {
        private Button loadFileButton, exportButton;
        private ComboBox algorithmSelector;
        private TabControl tabControl;
        private TextBox objectiveTextBox, constraintsTextBox;
        private OpenFileDialog openFileDialog;
        private CheckBox darkModeToggle;
		private Button solveButton;


		public MainForm()
        {
            Text = "OptiSolve - Linear Program Solver";
            Width = 950;
            Height = 650;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            loadFileButton = new Button
            {
                Text = "📂 Load Input File",
                Width = 180,
                Height = 35,
                Top = 20,
                Left = 20,
                BackColor = Color.SteelBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            loadFileButton.Click += LoadFileButton_Click;

            algorithmSelector = new ComboBox
            {
                Top = 20,
                Left = 220,
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            algorithmSelector.Items.AddRange(new string[]
            {
                "Primal Simplex",
                "Revised Simplex",
                "Branch & Bound",
                "Cutting Plane",
                "Knapsack (B&B)"
            });
            algorithmSelector.SelectedIndex = 0;

            exportButton = new Button
            {
                Text = "💾 Export",
                Width = 120,
                Height = 35,
                Top = 20,
                Left = 490,
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            exportButton.Click += ExportButton_Click;

            darkModeToggle = new CheckBox
            {
                Text = "🌙 Dark Mode",
                Top = 25,
                Left = 630,
                Font = new Font("Segoe UI", 9)
            };
            darkModeToggle.CheckedChanged += ToggleDarkMode;

            tabControl = new TabControl
            {
                Top = 80,
                Left = 20,
                Width = 880,
                Height = 480,
                Font = new Font("Segoe UI", 10)
            };

            objectiveTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10)
            };

			solveButton = new Button
			{
				Text = "🧮 Solve Simplex",
				Width = 160,
				Height = 35,
				Top = 20,
				Left = 770,
				BackColor = Color.DarkSlateBlue,
				ForeColor = Color.White,
				FlatStyle = FlatStyle.Flat
			};
			solveButton.Click += SolveButton_Click;
			Controls.Add(solveButton);


			constraintsTextBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10)
            };

            var tab1 = new TabPage("📈 Objective Function");
            var tab2 = new TabPage("📊 Constraints");

            tab1.Controls.Add(objectiveTextBox);
            tab2.Controls.Add(constraintsTextBox);

            tabControl.TabPages.Add(tab1);
            tabControl.TabPages.Add(tab2);

            Controls.Add(loadFileButton);
            Controls.Add(algorithmSelector);
            Controls.Add(exportButton);
            Controls.Add(darkModeToggle);
            Controls.Add(tabControl);

            openFileDialog = new OpenFileDialog();
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string[] lines = File.ReadAllLines(openFileDialog.FileName);
                    if (lines.Length < 3)
                        throw new Exception("Invalid file format");

                    objectiveTextBox.Text = lines[0];
                    constraintsTextBox.Text = string.Join(Environment.NewLine, lines, 1, lines.Length - 2);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading file: " + ex.Message);
                }
            }
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                FileName = "output.txt"
            };

            if (save.ShowDialog() == DialogResult.OK)
            {
				File.WriteAllText(save.FileName,
	                $"Algorithm: {algorithmSelector.SelectedItem}\n\n" +
	                $"Objective Function:\n{objectiveTextBox.Text}\n\n" +
	                $"Constraints:\n{constraintsTextBox.Text}");

			}
		}

		private void SolveButton_Click(object sender, EventArgs e)
		{
			// Hardcoded example tableau (2 vars, 2 constraints):
			double[,] tableau = new double[,]
			{
		{ 2, 3, 1, 0, 0, 100 },  // constraint 1
        { 4, 1, 0, 1, 0, 80 },   // constraint 2
        { -3, -5, 0, 0, 1, 0 }   // objective row (max)
			};

			string result = SimplexSolver.SolveMaxLP(tableau);
			MessageBox.Show(result, "Simplex Solver");
		}


		private void ToggleDarkMode(object sender, EventArgs e)
        {
            bool isDark = darkModeToggle.Checked;
            Color bg = isDark ? Color.FromArgb(30, 30, 30) : Color.White;
            Color fg = isDark ? Color.WhiteSmoke : Color.Black;

            BackColor = bg;
            foreach (Control ctrl in Controls)
            {
                ctrl.ForeColor = fg;
                if (!(ctrl is Button)) ctrl.BackColor = bg;
            }

            objectiveTextBox.BackColor = bg;
            objectiveTextBox.ForeColor = fg;
            constraintsTextBox.BackColor = bg;
            constraintsTextBox.ForeColor = fg;
        }
    }
}
