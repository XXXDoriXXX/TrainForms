using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using static TrainForms.Form1;

namespace TrainForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent(); 
            chart.Series.Clear();
            chart.Series.Add(series);
            guna2ComboBox1.SelectedIndex = 1;
        }
        public struct DataTrains
        {
            public int Id;
            public string FirstStation;
            public string SecondStation;
            public string MiddleStation;
            public TimeSpan StartTime;
            public TimeSpan StopTime;
            public double Distance;
        }
        private List<DataTrains> trainDataList = new List<DataTrains>();
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        int n = 0;
        public void newComboBox(int  check)
        {
            n = 0;
            DataComboBox.Items.Clear();

            for (int i = 0; i < Datagrid.Rows.Count; i++)
            {
                object prizvValue = Datagrid.Rows[i].Cells[check-1].Value;
                if (prizvValue != null)
                {
                    string prizv = prizvValue.ToString();
                    DataComboBox.Items.Add(prizv);
                }
                n++;
            }
        }

        private void прочитатиЗФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt";
                openFileDialog.Title = "Відкрити файл";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var filePath = openFileDialog.FileName;

                    try
                    {
                        Datagrid.Rows.Clear();

                        var lines = File.ReadAllLines(filePath);
                        foreach (var line in lines)
                        {
                            var fields = line.Split('\t');
                            if (fields.Length >= 7)
                            {
                                try
                                {
                                    int id = int.Parse(fields[0]);
                                    string firstStation = fields[1];
                                    string secondStation = fields[2];
                                    string middleStation = fields[3];
                                    TimeSpan startTime = TimeSpan.Parse(fields[4]);
                                    TimeSpan stopTime = TimeSpan.Parse(fields[5]);
                                    double distance = Convert.ToDouble(fields[6]);

                                    DataTrains tmp = new DataTrains
                                    {
                                        Id = id,
                                        FirstStation = firstStation,
                                        SecondStation = secondStation,
                                        MiddleStation = middleStation,
                                        StartTime = startTime,
                                        StopTime = stopTime,
                                        Distance = distance
                                    };

                                    trainDataList.Add(tmp);
                                    Datagrid.Rows.Add(tmp.Id, tmp.FirstStation, tmp.SecondStation, tmp.MiddleStation, tmp.StartTime, tmp.StopTime, tmp.Distance);
                                }
                                catch (FormatException fe)
                                {
                                    MessageBox.Show($"Помилка у форматі даних в рядку: {line}\n{fe.Message}", "Помилка формату", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"Неправильний формат рядка: {line}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Сталася помилка при читанні файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        int check = 1;
        int checkchange()
        {
            if (rd1.Checked)
            {
                check = 1;
            }
            else if (rd2.Checked)
            {
                check = 2;
            }
            else if (rd3.Checked)
            {
                check = 3;
            }
            else if (rd4.Checked)
            {
                check = 4;
            }
            else if (rd5.Checked)
            {
                check = 5;
            }
            else if (rd6.Checked)
            {
                check = 6;
            }
            else if (rd7.Checked)
            {
                check = 7;
            }
            return check;
        }
        private void tabPage2_Click(object sender, EventArgs e)
        {

            check = checkchange();
            newComboBox(check);
            //switch (check)
            //{
            //    case 1:

            //        break;
            //    case 2:

            //        break;
            //    case 3:

            //        break;
            //    case 4:

            //        break;
            //    case 5:

            //        break;
            //    case 6:

            //        break;
            //    case 7:

            //        break;
                
            //}
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            check = checkchange();
            newComboBox(check);
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            DataComboBox.Text = guna2TextBox1.Text;
        }
        int selectedRowIndex = -1;int i = -1;
        private void SearchBtn_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Text = DataComboBox.Text;
            string searchValue = guna2TextBox1.Text.Trim();
            bool found = false;


            listBox1.Items.Clear();

            try
            {
                foreach (DataGridViewRow row in Datagrid.Rows)
                {
                    if (row.IsNewRow) continue; 

                    if (row.Cells[check - 1].Value != null &&
                        row.Cells[check - 1].Value.ToString().Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        selectedRowIndex = row.Index;
                        string item = $"{row.Cells[0].Value.ToString()}" +
                                      $" - {row.Cells[1].Value.ToString()}" +
                                      $" - {row.Cells[2].Value.ToString()}" +
                                      $" - {row.Cells[3].Value.ToString()}" +
                                      $" - {row.Cells[4].Value.ToString()}" +
                                      $" - {row.Cells[5].Value.ToString()}" +
                                      $" - {row.Cells[6].Value.ToString()}"+
                                      $" - {selectedRowIndex.ToString()}";
                        listBox1.Items.Add(item);

                      
                      
                        found = true;
                    }
                }

                if (!found)
                {
                    MessageBox.Show("Елемент не знайдено.", "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2TextBox2.Clear();
                    guna2TextBox3.Clear();
                    guna2TextBox4.Clear();
                    guna2TextBox5.Clear();
                    guna2TextBox6.Clear();
                    guna2TextBox7.Clear();
                }
                else if (listBox1.Items.Count > 1)
                {
                    listBox1.Visible = true;
                    MessageBox.Show($"{listBox1.Items.Count} збіг(ів) знайдено.", "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    guna2TextBox2.Text = Datagrid.Rows[selectedRowIndex].Cells[1].Value.ToString();
                    guna2TextBox3.Text = Datagrid.Rows[selectedRowIndex].Cells[2].Value.ToString();
                    guna2TextBox4.Text = Datagrid.Rows[selectedRowIndex].Cells[3].Value.ToString();
                    guna2TextBox5.Text = Datagrid.Rows[selectedRowIndex].Cells[4].Value.ToString();
                    guna2TextBox6.Text = Datagrid.Rows[selectedRowIndex].Cells[5].Value.ToString();
                    guna2TextBox7.Text = Datagrid.Rows[selectedRowIndex].Cells[6].Value.ToString();
                    guna2TextBox8.Text = Datagrid.Rows[selectedRowIndex].Cells[0].Value.ToString();
                    listBox1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Дані користувача невірні! {ex.Message}", "Результат пошуку", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2ShadowPanel1_MouseEnter(object sender, EventArgs e)
        {

        }
        bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        private void guna2ShadowPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = System.Windows.Forms.Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void guna2ShadowPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(System.Windows.Forms.Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void guna2ShadowPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                string[] selectedValues = listBox1.SelectedItem.ToString().Split(new string[] { " - " }, StringSplitOptions.None);

                if (selectedValues.Length == 8)
                {
                    guna2TextBox2.Text = selectedValues[1];
                    guna2TextBox3.Text = selectedValues[2];
                    guna2TextBox4.Text = selectedValues[3];
                    guna2TextBox5.Text = selectedValues[4];
                    guna2TextBox6.Text = selectedValues[5];
                    guna2TextBox7.Text = selectedValues[6];
                    guna2TextBox8.Text = selectedValues[0];
                    selectedRowIndex = Convert.ToInt32(selectedValues[7]);
                } }               
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (DataComboBox.Items.Count > 0 && DataComboBox.SelectedIndex > 0)
                {
                    DataComboBox.SelectedIndex--;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Сталася помилка: " + ex.Message);
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (DataComboBox.Items.Count > 0 && DataComboBox.SelectedIndex < DataComboBox.Items.Count - 1)
                {
                    DataComboBox.SelectedIndex++;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Сталася помилка: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!string.IsNullOrWhiteSpace(guna2TextBox2.Text) && !string.IsNullOrWhiteSpace(guna2TextBox3.Text) && !string.IsNullOrWhiteSpace(guna2TextBox4.Text) && !string.IsNullOrWhiteSpace(guna2TextBox5.Text) &&!string.IsNullOrWhiteSpace(guna2TextBox6.Text) && !string.IsNullOrWhiteSpace(guna2TextBox7.Text) && !string.IsNullOrWhiteSpace(guna2TextBox8.Text) )&&Convert.ToDouble(guna2TextBox7.Text)>0)
                {

                    Datagrid.Rows[selectedRowIndex].Cells[0].Value = guna2TextBox8.Text;
                    Datagrid.Rows[selectedRowIndex].Cells[1].Value = guna2TextBox2.Text;
                    Datagrid.Rows[selectedRowIndex].Cells[2].Value = guna2TextBox3.Text;
                    Datagrid.Rows[selectedRowIndex].Cells[3].Value = guna2TextBox4.Text;
                    Datagrid.Rows[selectedRowIndex].Cells[4].Value = guna2TextBox5.Text;
                    Datagrid.Rows[selectedRowIndex].Cells[5].Value = guna2TextBox6.Text;
                    Datagrid.Rows[selectedRowIndex].Cells[6].Value = guna2TextBox7.Text;
                    trainDataList[selectedRowIndex] = new DataTrains
                    {
                        Id = int.Parse(guna2TextBox8.Text),
                        FirstStation = guna2TextBox2.Text,
                        SecondStation = guna2TextBox3.Text,
                        MiddleStation = guna2TextBox4.Text,
                        StartTime = TimeSpan.Parse(guna2TextBox5.Text),
                        StopTime = TimeSpan.Parse(guna2TextBox6.Text),
                        Distance = double.Parse(guna2TextBox7.Text)
                    };







                    newComboBox(check);

                    MessageBox.Show("Запис успішно змінено");
                }
                else if (DataComboBox.SelectedIndex < 0)
                {
                    MessageBox.Show("Такого користувача не знайдено!");
                }
                else MessageBox.Show("Введіть дані!");
            }
            catch
            {
                MessageBox.Show("Невідома помилка, спробуйте ще раз!");
            }

        }
        void masktime(object sender, KeyPressEventArgs e)
        {
            Guna2TextBox textBox = sender as Guna2TextBox;

          
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ':'))
            {
                e.Handled = true;
                return;
            }
            else
            {
                if ((textBox.Text.Length == 2 || textBox.Text.Length == 5||textBox.Text.Length>6) && !(char.IsControl(e.KeyChar)))
                {
                    int selectionStart = textBox.SelectionStart;

                    if (textBox.Text.Length == 2)
                    {
                        if ((Convert.ToInt32(textBox.Text) > 24))
                        {
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            textBox.Text += ":";
                            selectionStart++;
                            textBox.SelectionStart = selectionStart;
                        }
                    }

                    if (textBox.Text.Length == 5)
                    {
                        if ((Convert.ToInt32(textBox.Text.Substring(3)) > 59))
                        {
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                            textBox.Text += ":";
                            selectionStart++;

                            textBox.SelectionStart = selectionStart;
                        }
                    }
                    if (textBox.Text.Length ==7)
                    {
                        if ((Convert.ToInt32(textBox.Text.Substring(6)) > 5))
                        {
                            e.Handled = true;
                            return;
                        }
                        else
                        {
                           
                            selectionStart++;

                            textBox.SelectionStart = selectionStart;
                        }
                    }
                }
           

            }
            textBox.SelectionStart = textBox.Text.Length;
            if ((char.IsDigit(e.KeyChar) && textBox.Text.Length < 8) || char.IsControl(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void зберегтиФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();

        }
        private void SaveData()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {

                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveFileDialog.Title = "Зберегти файл як";


                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {

                    var filePath = saveFileDialog.FileName;

                    try
                    {

                        using (var file = new StreamWriter(filePath))
                        {
                            for (int i = 0; i < Datagrid.Rows.Count - 1; i++)
                            {

                                var number = Datagrid.Rows[i].Cells[0].Value?.ToString() ?? string.Empty;
                                var startst = Datagrid.Rows[i].Cells[1].Value?.ToString() ?? string.Empty;
                                var stopst = Datagrid.Rows[i].Cells[2].Value?.ToString() ?? string.Empty;
                                var midlest = Datagrid.Rows[i].Cells[3].Value?.ToString() ?? string.Empty;
                                var starttim = Datagrid.Rows[i].Cells[4].Value?.ToString() ?? string.Empty;
                                var stoptim = Datagrid.Rows[i].Cells[5].Value?.ToString() ?? string.Empty;
                                var distance = Datagrid.Rows[i].Cells[6].Value?.ToString() ?? string.Empty;

                                if (string.IsNullOrEmpty(number) || string.IsNullOrEmpty(startst) ||
                                    string.IsNullOrEmpty(stopst) || starttim.Length < 8 || stoptim.Length < 8 || string.IsNullOrEmpty(midlest) || string.IsNullOrEmpty(distance))
                                {
                                    File.Delete( saveFileDialog.FileName);
                                    MessageBox.Show("Всі поля повинні бути заповнені. Дані не збережено.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                               

                                file.WriteLine($"{number}" +
                                    $"\t{startst}" +
                                    $"\t{stopst}" +
                                    $"\t{midlest}"+
                                    $"\t{starttim}" +
                                    $"\t{stoptim}" +
                                    $"\t{distance}");
                            }

                            MessageBox.Show("Дані успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Сталася помилка при збереженні файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        int currentRecordNumber = 1;
        private void guna2Button5_Click(object sender, EventArgs e)
        {
       
            if (string.IsNullOrWhiteSpace(txtTrainId.Text) ||
                string.IsNullOrWhiteSpace(txtFirstStation.Text) ||
                string.IsNullOrWhiteSpace(txtSecondStation.Text) ||
                string.IsNullOrWhiteSpace(txtStartTime.Text) ||
                string.IsNullOrWhiteSpace(txtStopTime.Text) ||
                string.IsNullOrWhiteSpace(txtDistance.Text) ||
                !int.TryParse(txtTrainId.Text, out int trainId) ||
                !TimeSpan.TryParse(txtStartTime.Text, out TimeSpan startTime) ||
                !TimeSpan.TryParse(txtStopTime.Text, out TimeSpan stopTime) ||
                !double.TryParse(txtDistance.Text.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out double distance))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля правильно.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (startTime < TimeSpan.Zero || startTime >= new TimeSpan(24, 0, 0) ||
        stopTime < TimeSpan.Zero || stopTime >= new TimeSpan(24, 0, 0))
            {
                MessageBox.Show("Час відправлення та час прибуття мають бути в межах від 00:00 до 23:59.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int rowIndex = Datagrid.Rows.Add();
            DataGridViewRow row = Datagrid.Rows[rowIndex];
            row.Cells[0].Value = trainId;
            row.Cells[1].Value = txtFirstStation.Text;
            row.Cells[2].Value = txtSecondStation.Text;
            row.Cells[3].Value = txtMiddleStation.Text;
            row.Cells[4].Value = startTime;
            row.Cells[5].Value = stopTime;
            row.Cells[6].Value = distance;
            DataGridViewRow prerow = PreVievDatagrid.Rows[rowIndex];
           prerow.Cells[0].Value = trainId;
           prerow.Cells[1].Value = txtFirstStation.Text;
           prerow.Cells[2].Value = txtSecondStation.Text;
           prerow.Cells[3].Value = txtMiddleStation.Text;
           prerow.Cells[4].Value = startTime;
           prerow.Cells[5].Value = stopTime;
           prerow.Cells[6].Value = distance;
            if (txtRecordNumber.Text == "1")
            {
                List<DataTrains> trainDataList = new List<DataTrains>();
            }
            trainDataList.Add(new DataTrains
            {
                Id = trainId,
                FirstStation = txtFirstStation.Text,
                SecondStation = txtSecondStation.Text,
                MiddleStation = txtMiddleStation.Text,
                StartTime = startTime,
                StopTime = stopTime,
                Distance = distance
            });
            txtTrainId.Clear();
            txtFirstStation.Clear();
            txtSecondStation.Clear();
            txtMiddleStation.Clear();
            txtStartTime.Clear();
            txtStopTime.Clear();
            txtDistance.Clear();

            currentRecordNumber++;
            txtRecordNumber.Text = currentRecordNumber.ToString();
        }
        void sortaya(int check)
        {
            Datagrid.Columns[check - 1].SortMode = DataGridViewColumnSortMode.Programmatic;
            Datagrid.Sort(Datagrid.Columns[check - 1], ListSortDirection.Ascending);
            newComboBox(check);
        }
        void sortyaa(int check)
        {
            Datagrid.Columns[check - 1].SortMode = DataGridViewColumnSortMode.Programmatic;
            Datagrid.Sort(Datagrid.Columns[check - 1], ListSortDirection.Descending);
            newComboBox(check);
        }
        private void номерToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
              
                string text = menuItem.Text;
                switch (text)
                {
                    case "Номер":
                        check = 1;
                        sortyaa(check);
                        break;
                    case "Початкова станція":
                        check = 2;
                        sortyaa(check);
                        break;
                    case "Кінцева станція":
                        check = 3;
                        sortyaa(check);
                        break;
                    case "Проїздна станція":
                        check = 4;
                        sortyaa(check);
                        break;
                    case "Час відправлення":
                        check = 5;
                        sortyaa(check);
                        break;
                    case "Час прибуття":
                        check = 6;
                        sortyaa(check);
                        break;
                    case "Відстань":
                        check = 7;
                        sortyaa(check);
                        break;
                    default:
                        break;
                }
           
            }
        }
        Series series = new Series
        {
            Name = "Швидкість",
            Color = System.Drawing.Color.DeepSkyBlue,
            ChartType = SeriesChartType.Column
        };
      
        private void CalculateAndDisplayAverageSpeed()
        {
            series.Points.Clear();
            SpeedDatagrid.Rows.Clear();
            chart.ChartAreas[0].AxisY.Minimum = 50;
            chart.ChartAreas[0].AxisY.Maximum = 150;
            foreach (var train in trainDataList)
            {
                double totalHours;
                if (train.StopTime < train.StartTime)
                {
                    totalHours = (train.StopTime.Add(new TimeSpan(24, 0, 0)) - train.StartTime).TotalHours;
                }
                else
                {
                    totalHours = (train.StopTime - train.StartTime).TotalHours;
                }

                double averageSpeed = train.Distance / totalHours;
                series.Points.AddXY($"Потяг {train.Id}", averageSpeed);
              

            }
            foreach (var train in trainDataList)
            {
                double totalHours;
                if (train.StopTime < train.StartTime)
                {
                    totalHours = (train.StopTime.Add(new TimeSpan(24, 0, 0)) - train.StartTime).TotalHours;
                }
                else
                {
                    totalHours = (train.StopTime - train.StartTime).TotalHours;
                }

                double averageSpeed = train.Distance / totalHours;

             
                int rowIndex = SpeedDatagrid.Rows.Add();
                SpeedDatagrid.Rows[rowIndex].Cells[0].Value = train.Id;
                SpeedDatagrid.Rows[rowIndex].Cells[1].Value = averageSpeed;
            }


            chart.Titles.Clear();
            chart.ChartAreas[0].AxisY.Title = "Середня швидкість (км/год)";
        }
        

        private void номерToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {

                string text = menuItem.Text;
                switch (text)
                {
                    case "Номер":
                        check = 1;
                        sortaya(check);
                        break;
                    case "Початкова станція":
                        check = 2;
                        sortaya(check);
                        break;
                    case "Кінцева станція":
                        check = 3;
                        sortaya(check);
                        break;
                    case "Проїздна станція":
                        check = 4;
                        sortaya(check);
                        break;
                    case "Час відправлення":
                        check = 5;
                        sortaya(check);
                        break;
                    case "Час прибуття":
                        check = 6;
                        sortaya(check);
                        break;
                    case "Відстань":
                        check = 7;
                        sortaya(check);
                        break;
                    default:
                        break;
                }

            }
        }
        
        private void середняШвидкістьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            CalculateAndDisplayAverageSpeed();
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

            switch (guna2TabControl1.SelectedIndex)
            {
                case 2:
                   
                    break;
                case 3:
                CalculateAndDisplayAverageSpeed();
                    break;
                case 4:
                    fillcombobx();
                    break;
            }
        }
        private void fillcombobx()
        {
            n = 0;
            ComboBoxFirst.Items.Clear();

            for (int i = 0; i < Datagrid.Rows.Count; i++)
            {
                object firsstation = Datagrid.Rows[i].Cells[1].Value;
                if (firsstation != null)
                {
                    string first = firsstation.ToString();
                    if (!ComboBoxFirst.Items.Contains(first))
                    {
                        ComboBoxFirst.Items.Add(first);
                    }
                }
                n++;
            }

            ComboBoxFirst.SelectedIndexChanged += ComboBoxFirst_SelectedIndexChanged;
        }

        private void ComboBoxFirst_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBoxSecond.Items.Clear();

            string selectedFirstStation = ComboBoxFirst.SelectedItem.ToString();

            for (int i = 0; i < Datagrid.Rows.Count; i++)
            {
                object firsstation = Datagrid.Rows[i].Cells[1].Value;
                object secondstation = Datagrid.Rows[i].Cells[2].Value;

                if (firsstation != null && secondstation != null)
                {
                    string first = firsstation.ToString();
                    string second = secondstation.ToString();

                    if (first == selectedFirstStation && !ComboBoxSecond.Items.Contains(second))
                    {
                        ComboBoxSecond.Items.Add(second);
                    }
                }
            }
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (guna2ComboBox1.SelectedIndex)
            {
                case 0:
                    series.ChartType = SeriesChartType.Column;
                    break;
                case 1:
                    series.ChartType = SeriesChartType.Line;
                    break;
                case 2:
                    series.ChartType = SeriesChartType.Pie;

                    break;
                case 3:
                    series.ChartType = SeriesChartType.Point;

                    break;
                case 4:
                    series.ChartType = SeriesChartType.Area;
                    break;
            }
        }
        int scrolvalue;
        private void guna2VScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            Guna2VScrollBar scrollBar = sender as Guna2VScrollBar;

            if (scrollBar != null)
            {
                int scrollValue = scrollBar.Value;

           
                scrolvalue = scrollValue;
                chart.ChartAreas[0].AxisY.Minimum = 50+scrolvalue;
                chart.ChartAreas[0].AxisY.Maximum = 150+scrolvalue;
              
            }
        }

        private void GrupComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            GrupDatagrid.Rows.Clear();

           
            string selectedFirstStation = ComboBoxFirst.SelectedItem?.ToString();
            string selectedSecondStation = ComboBoxSecond.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(selectedFirstStation) || string.IsNullOrEmpty(selectedSecondStation))
            {
                MessageBox.Show("Будь ласка, виберіть початкову та кінцеву станції.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<DataTrains> groupedData = new List<DataTrains>();
            foreach (var train in trainDataList)
            {
                if (train.FirstStation == selectedFirstStation && train.SecondStation == selectedSecondStation)
                {
                    groupedData.Add(train);
                }
            }
            foreach (var train in groupedData)
            {
                int rowIndex = GrupDatagrid.Rows.Add();
                DataGridViewRow row = GrupDatagrid.Rows[rowIndex];
                row.Cells[0].Value = train.Id;
                row.Cells[1].Value = train.FirstStation;
                row.Cells[2].Value = train.SecondStation;
                row.Cells[3].Value = train.MiddleStation;
                row.Cells[4].Value = train.StartTime;
                row.Cells[5].Value = train.StopTime;
                row.Cells[6].Value = train.Distance;
            }

        
            if (groupedData.Count == 0)
            {
                MessageBox.Show("Записи, що відповідають вибраним станціям, не знайдено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            GrupDatagrid.Rows.Clear();
            var groupedData = trainDataList
                .GroupBy(t => new { t.FirstStation, t.SecondStation })
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    SecondStation = g.Key.FirstStation,
                    MiddleStation = g.Key.SecondStation,
                    Trains = g.ToList()
                });
            foreach (var group in groupedData)
            {
                foreach (var train in group.Trains)
                {
                    int rowIndex = GrupDatagrid.Rows.Add();
                    DataGridViewRow row = GrupDatagrid.Rows[rowIndex];
                    row.Cells[0].Value = train.Id;
                    row.Cells[1].Value = train.FirstStation;
                    row.Cells[2].Value = train.SecondStation;
                    row.Cells[3].Value = train.MiddleStation;
                    row.Cells[4].Value = train.StartTime;
                    row.Cells[5].Value = train.StopTime;
                    row.Cells[6].Value = train.Distance;
                }
            }
        }

        private void DataComboBox_Click(object sender, EventArgs e)
        {
            newComboBox(check);
        }

        private void txtTrainId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtFirstStation_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar)&&!(e.KeyChar==','))
            {
                e.Handled = true;
                return;
            }
        }

        private void txtDistance_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !(e.KeyChar == ','))
            {
                e.Handled = true;
                return;
            }
        }

        private void очиститиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            trainDataList.Clear();
            Datagrid.Rows.Clear();
            newComboBox(check);
        }

        private void очиститиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guna2TextBox1.Clear();
            guna2TextBox2.Clear();
            guna2TextBox3.Clear();
            guna2TextBox4.Clear();
            guna2TextBox5.Clear();
            guna2TextBox6.Clear();
            guna2TextBox7.Clear();
            guna2TextBox8.Clear();
            listBox1.Items.Clear();
            DataComboBox.Items.Clear();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            if (DataComboBox.SelectedItem != null)
            {

                string searchValue = guna2TextBox1.Text.Trim();

               
                int rowToDeleteIndex = -1;

                foreach (DataGridViewRow row in Datagrid.Rows)
                {
                    if (row.Cells[check-1].Value != null && row.Cells[check-1].Value.ToString().Equals(searchValue, StringComparison.OrdinalIgnoreCase))
                    {
                        rowToDeleteIndex = row.Index;
                        string item = $"{row.Cells[0].Value.ToString()} - {row.Cells[1].Value.ToString()} - {row.Cells[2].Value.ToString()} - {row.Cells[3].Value.ToString()} - {row.Cells[4].Value.ToString()} - {row.Cells[5].Value.ToString()} - {row.Cells[6].Value.ToString()}";
                        listBox1.Items.Remove(item);
                        int id = int.Parse(row.Cells[0].Value.ToString()); 
                        if (int.TryParse(row.Cells[0].Value.ToString(), out int id1))
                        {
                            int indexToRemove = trainDataList.FindIndex(train => train.Id == id1);
                            if (indexToRemove != -1)
                            {
                                trainDataList.RemoveAt(indexToRemove);
                            }
                        }
                        break;
                    }
                }


                if (rowToDeleteIndex != -1)
                {
                    Datagrid.Rows.RemoveAt(rowToDeleteIndex);
                    DataComboBox.Items.Remove(DataComboBox.SelectedItem);

                    newComboBox(check);
                    guna2TextBox1.Text = "";
                    MessageBox.Show("Статус: успішно!", "Видалення", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("Статус: невдало!", "Видалення", MessageBoxButtons.OK);
                }
            }

        }

        private void проПрограмуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form2 = new About();
            form2.Show();
        }
    }
}