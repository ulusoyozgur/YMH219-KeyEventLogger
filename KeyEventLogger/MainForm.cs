namespace KeyEventLogger;

public sealed class MainForm : Form
{
    private readonly KeyRecorder recorder = new();
    private readonly ListBox history = new() { Dock = DockStyle.Fill, IntegralHeight = false,
        Font = new Font("Consolas", 11), HorizontalScrollbar = true, TabStop = false };
    private readonly Label status = new() { AutoSize = true, Padding = new Padding(0, 12, 0, 0) };
    private readonly Button start = new() { Text = "Başlat", Width = 110, Height = 40 };
    private readonly Button stop = new() { Text = "Durdur", Width = 110, Height = 40 };
    private readonly Button clear = new() { Text = "Temizle", Width = 110, Height = 40 };
    private readonly Button save = new() { Text = "TXT Kaydet", Width = 130, Height = 40 };
    private readonly TextBox input = new() { Dock = DockStyle.Fill, Multiline = true,
        AcceptsTab = true, AcceptsReturn = true, ScrollBars = ScrollBars.Vertical };

    public MainForm()
    {
        Text = "Key Event Logger | YMH219";
        ClientSize = new Size(820, 650);
        MinimumSize = new Size(840, 550);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 11);
        BackColor = Color.FromArgb(242, 245, 249);
        KeyPreview = true;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(24), ColumnCount = 1, RowCount = 6 };
        foreach (var height in new[] { 48, 52, 52, 90, 34 }) layout.RowStyles.Add(new RowStyle(SizeType.Absolute, height));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.Controls.Add(new Label { Text = "Key Event Logger", AutoSize = true,
            Font = new Font("Segoe UI", 22, FontStyle.Bold) }, 0, 0);
        layout.Controls.Add(new Label { Text = "Yalnızca bu pencere aktifken basılan tuşları kaydeder.\nBaşlat'a basıp aşağıdaki deneme alanına yazabilirsin.", Dock = DockStyle.Fill }, 0, 1);
        var toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill };
        toolbar.Controls.AddRange([start, stop, clear, save, status]);
        layout.Controls.Add(toolbar, 0, 2);
        input.PlaceholderText = "Tuşları burada dene…";
        layout.Controls.Add(input, 0, 3);
        layout.Controls.Add(new Label { Text = "SAAT                    TUŞ", AutoSize = true, Padding = new Padding(0, 8, 0, 0) }, 0, 4);
        layout.Controls.Add(history, 0, 5);
        Controls.Add(layout);
        start.Click += (_, _) => { recorder.Start(); UpdateStatus(); input.Focus(); };
        stop.Click += (_, _) => { recorder.Stop(); UpdateStatus(); };
        clear.Click += (_, _) =>
        {
            recorder.Clear();
            history.Items.Clear();
            input.Clear();
            UpdateStatus();
        };
        save.Click += (_, _) => SaveRecords();
        input.TextChanged += (_, _) => UpdateStatus();
        input.PreviewKeyDown += (_, e) =>
        {
            if (e.KeyCode is Keys.Tab or Keys.Left or Keys.Right or Keys.Up or Keys.Down) e.IsInputKey = true;
        };
        KeyDown += (_, e) =>
        {
            if (ContainsFocus && Form.ActiveForm == this) recorder.Record(e.KeyData);
        };
        recorder.EntryRecorded += entry =>
        {
            if (history.Items.Count == KeyRecorder.MaximumEntries) history.Items.RemoveAt(0);
            history.Items.Add(entry);
            history.TopIndex = history.Items.Count - 1;
            UpdateStatus();
        };
        UpdateStatus();
    }

    private void UpdateStatus()
    {
        start.Enabled = !recorder.IsRecording;
        stop.Enabled = recorder.IsRecording;
        clear.Enabled = recorder.Entries.Count > 0 || input.TextLength > 0;
        save.Enabled = recorder.Entries.Count > 0;
        status.Text = recorder.IsRecording ? "● Kayıt açık" : "● Kayıt kapalı";
        status.ForeColor = recorder.IsRecording ? Color.DarkGreen : Color.DimGray;
    }

    private void SaveRecords()
    {
        recorder.Stop();
        UpdateStatus();
        using var dialog = new SaveFileDialog
        {
            Title = "Tuş kayıtlarını kaydet",
            Filter = "Metin dosyası (*.txt)|*.txt",
            DefaultExt = "txt",
            AddExtension = true,
            FileName = $"tus-kayitlari-{DateTime.Now:yyyyMMdd-HHmmss}.txt"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            recorder.Save(dialog.FileName);
            MessageBox.Show(this, "Kayıtlar kaydedildi.", "Kaydet", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            MessageBox.Show(this, "Dosya kaydedilemedi. Yazma izni olan başka bir konum seçin.",
                "Kaydetme hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
