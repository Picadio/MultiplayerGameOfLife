using MGOL.Client.packets.impl;
using MGOL.Client.algorithm;
using Timer = System.Windows.Forms.Timer;

namespace MGOL.Client;

public sealed partial class MainForm : Form
{
    private const int CellSize = 15;
    private const int ButtonWidth = 150; 
    private const int ButtonHeight = 30;
    private const int ButtonSpacing = 10; 
    
    private IAlgorithm<int> _algorithm;
    public Timer Timer { get; }
    public TextBox IpTextBox { get; private set; }
    public Label IsConnectedLabel { get; private set; }
    public Label CurrentStepLabel { get; private set; }
    public MainForm(IAlgorithm<int> algorithm)
    {
        _algorithm = algorithm;
        DoubleBuffered = true;
        ClientSize = new Size(_algorithm.Cols * CellSize + 200, _algorithm.Rows * CellSize);
        Text = "Гра Життя";
        BackColor = Color.White;
        Timer = new Timer { Interval = 100 };
        Timer.Tick += (_, _) => NextStep();
        
        DrawServerButtons();
        DrawGameButtons();

        MouseClick += (_, e) => HandleMouseClick(e.X, e.Y);
        Paint += (_, e) => DrawGrid(e.Graphics);
        FormClosing += (_, _) => Program.Client.Stop();
    }

    private void DrawServerButtons()
    {
        var createLobbyButton = new Button
        {
            Text = "Створити",
            Location = new Point(_algorithm.Cols * CellSize + 10, ClientSize.Height - (ButtonHeight + ButtonSpacing) * 3),
            Size = new Size(100, 30)
        };
        var connectButton = new Button
        {
            Text = "Приєднатися",
            Location = new Point(_algorithm.Cols * CellSize + 10, ClientSize.Height - (ButtonHeight + ButtonSpacing) * 2),
            Size = new Size(120, 30)
        };
        IpTextBox = new TextBox
        {
            Text = "",
            Location = new Point(_algorithm.Cols * CellSize + 10, ClientSize.Height - (ButtonHeight + ButtonSpacing) * 1),
            Size = new Size(150, 30)
        };
        
        createLobbyButton.Click += async (_, _) => 
        {
            if (Program.Client.IsConnected()) return;
            var packet = new CreateSessionPacket();
            await Program.Client.SendAsync(packet);
        };
        connectButton.Click += async (_, _) =>
        {
            if (Program.Client.IsConnected()) return;

            if (Guid.TryParse(IpTextBox.Text, out Guid uuid))
            {
                var packet = new JoinSessionPacket(uuid);
                await Program.Client.SendAsync(packet);
            }
            else
            {
                MessageBox.Show("Введіть корректний Id", "Невірний Id сесії", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };
        
        Controls.Add(connectButton);
        Controls.Add(createLobbyButton);
    }
    
    private void DrawGameButtons()
    {
        var startButton = new Button
        {
            Text = "▶ Старт / Стоп",
            Location = new Point(_algorithm.Cols * CellSize + 10, 5),
            Size = new Size(ButtonWidth, ButtonHeight)
        };
        var nextStepButton = new Button
        {
            Text = "Наступний крок",
            Location = new Point(_algorithm.Cols * CellSize + 10, ButtonHeight + ButtonSpacing),
            Size = new Size(ButtonWidth, ButtonHeight)
        };
        var resetButton = new Button
        {
            Text = "Скидання",
            Location = new Point(_algorithm.Cols * CellSize + 10, (ButtonHeight + ButtonSpacing) * 2),
            Size = new Size(ButtonWidth, ButtonHeight)
        };
        var speedTrackBar = new TrackBar
        {
            Minimum = 10,
            Maximum = 500,
            Value = 100,
            Location = new Point(_algorithm.Cols * CellSize + 10, (ButtonHeight + ButtonSpacing) * 4),
            Size = new Size(ButtonWidth, 30)
        };
        IsConnectedLabel = new Label()
        {
            Text = $"Статус: disconnected",
            Location = new Point(_algorithm.Cols * CellSize + 10, ClientSize.Height - (ButtonHeight + ButtonSpacing) * 4),
            Size = new Size(ButtonWidth, ButtonHeight)
        };
        CurrentStepLabel = new Label()
        {
            Text = $"Ітерація: 0",
            Location = new Point(_algorithm.Cols * CellSize + 10, (ButtonHeight + ButtonSpacing) * 6),
            Size = new Size(ButtonWidth, ButtonHeight)
        };
        
        nextStepButton.Click += async (_, _) =>
        {
            if (!Program.Client.IsConnected())
            {
                MessageBox.Show("Увійдіть або створіть сессію", "Немає сессії", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var packet = new NextStepPacket();
            await Program.Client.SendAsync(packet);
            NextStep();
        };
        startButton.Click += async (_, _) =>
        {
            if (!Program.Client.IsConnected())
            {
                MessageBox.Show("Увійдіть або створіть сессію", "Немає сессії", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var packet = new StartGamePacket(!Timer.Enabled);
            await Program.Client.SendAsync(packet);
            Timer.Enabled = !Timer.Enabled;
            if (!Timer.Enabled)
            {
                var packet2 = new SyncPacket(_algorithm.Step, _algorithm.CurrentGrid);
                await Program.Client.SendAsync(packet2);
            }
        };
        resetButton.Click += async (_, _) =>
        {
            if (!Program.Client.IsConnected())
            {
                MessageBox.Show("Увійдіть або створіть сессію", "Немає сессії", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var packet = new ResetPacket();
            await Program.Client.SendAsync(packet);
            Reset();
        };
        speedTrackBar.Scroll += async (_, _) =>
        {
            if (!Program.Client.IsConnected())
            {
                MessageBox.Show("Увійдіть або створіть сессію", "Немає сессії", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var packet = new ChangeSpeedPacket(speedTrackBar.Value);
            await Program.Client.SendAsync(packet);
            ChangeSpeed(speedTrackBar.Value);
        };
        
        Controls.Add(IsConnectedLabel);
        Controls.Add(nextStepButton);
        Controls.Add(startButton);
        Controls.Add(IpTextBox);
        Controls.Add(resetButton);
        Controls.Add(speedTrackBar);
        Controls.Add(CurrentStepLabel);
    }
    
    private void DrawGrid(Graphics g)
    {
        
        for (int x = 0; x < _algorithm.Rows; x++)
        for (int y = 0; y < _algorithm.Cols; y++)
        {
            
            var rect = new Rectangle(y * CellSize, x * CellSize, CellSize, CellSize);
            
            if (_algorithm.CurrentGrid[x, y] == 1)
                g.FillRectangle(Brushes.Blue, rect);
            if (_algorithm.CurrentGrid[x, y] == 2)
                g.FillRectangle(Brushes.Red, rect);
            g.DrawRectangle(Pens.LightGray, rect);
        }
    }

    private async Task HandleMouseClick(int mouseX, int mouseY)
    {
        int x = mouseY / CellSize;
        int y = mouseX / CellSize;
        
        if (!_algorithm.InBounds(x, y)) return;
        if(_algorithm.CurrentGrid[x, y] > 0) return;
        if (!Program.Client.IsConnected())
        {
            MessageBox.Show("Увійдіть або створіть сессію", "Немає сессії", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if(Timer.Enabled) return;
        
        var packet = new SetCellPacket(x, y);
        await Program.Client.SendAsync(packet);
        
        SetCell(x, y, 1);
    }

    public void SetCell(int x, int y, int value)
    {
        _algorithm.SetCellValue(x, y, value);
        Invalidate();
    }
    
    public void SwitchTimer(bool enable)
    {
        this.Invoke(() =>
        {
            Timer.Enabled = enable;
        });
    }
    
    public void SwitchTimer()
    {
        SwitchTimer(!Timer.Enabled);
    }
    
    public void ChangeSpeed(int newSpeed)
    {
        Invoke(() => Timer.Interval = newSpeed);
    }

    public void NextStep()
    {
        _algorithm.NextGeneration();
        CurrentStepLabel.Text = "Ітерація: " + _algorithm.Step;
        if (!_algorithm.IsChanged())
        {
            SwitchTimer(false);
        }
        Invalidate(); 
    }

    public void Reset()
    {
        Timer.Enabled = false;
        _algorithm.Reset();
        CurrentStepLabel.Text = "Ітерація: 0";
        Invalidate();
    }

    public void SetConnected(bool isConnected)
    {
        var text = IsConnectedLabel.Text;
        if(isConnected)
            IsConnectedLabel.Text = text.Replace("disconnected", "connected");
        else
            IsConnectedLabel.Text = text.Replace("connected", "disconnected");
        
    }
}