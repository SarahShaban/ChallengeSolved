using System;
using System.Collections.Generic;
using System.Windows;

namespace ManusChallengeSolved
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<List<int>> cities = new List<List<int>>();
        public List<int> test = new List<int>();

        public MainWindow()
        {
            InitializeComponent();
            if (cbFromCity.Items.Count == 0 && cbTargetCity.Items.Count == 0)
            {
                cbFromCity.Items.Insert(0, "From");
                cbFromCity.SelectedItem = cbFromCity.Items[0];
                cbTargetCity.Items.Insert(0, "To");
                cbTargetCity.SelectedItem = cbTargetCity.Items[0];
            }
        }

        public void GenerateCities(int numberOfCities)
        {
            Random random = new Random();
            List<int> cityZero = new List<int>();

            cities.Clear();
            cities.Add(cityZero);

            for (int i = 1; i < numberOfCities; i++)
            {
                List<int> connections = new List<int>();

                connections.Add(i - 1);//adds a connection to the previous city

                cities[i - 1].Add(i);//adds the the previous city a connection to the current city

                int rand = random.Next(0, (cities.Count - 1));

                cities[rand].Add(i);//chooses a random city and adds the current city in it's connections

                connections.Add(rand);
                cities.Add(connections);
                //return cities;
            }
        }

        public List<int> FindShortestPath(int start, int target)
        {
            if (start < cities.Count && target < cities.Count)
            {
                List<int> unvisited = new List<int>();
                List<List<int>> paths = new List<List<int>>();

                paths.Add(new List<int>());
                paths[0].Add(start);

                if (start == target)
                {
                    return paths[0];
                }

                while (paths.Count != 0)
                {
                    List<int> path = new List<int>();

                    path = paths[paths.Count - 1];
                    paths.Remove(path);
                    int lastStep = path[path.Count - 1];

                    if (!unvisited.Contains(lastStep))
                    {
                        List<int> neighbours = new List<int>();
                        neighbours = cities[lastStep];

                        foreach (int neighbour in neighbours)
                        {
                            List<int> neighbourPath = new List<int>();

                            neighbourPath = new List<int>(path);

                            neighbourPath.Add(neighbour);
                            paths.Add(neighbourPath);

                            if (neighbour == target)
                            {
                                return neighbourPath;
                            }
                        }
                        unvisited.Add(lastStep);
                    }
                }
            }
            return new List<int>();
        }

        private void buttBuild_Click(object sender, RoutedEventArgs e)
        {
            lbCities.Items.Clear();

            cbFromCity.Items.Clear();
            cbTargetCity.Items.Clear();

            int cityNumber = 0;

            if (int.TryParse(tbCityCount.Text, out int i))
            {
                GenerateCities(int.Parse(tbCityCount.Text));
                foreach (List<int> city in cities)
                {
                    lbCities.Items.Add("City " + cityNumber + ": " + string.Join(",", city));
                    cityNumber++;

                    cbFromCity.Items.Add("City " + (cityNumber - 1));
                    cbTargetCity.Items.Add("City " + (cityNumber - 1));
                }
                cbFromCity.SelectedItem = cbFromCity.Items[0];
                cbTargetCity.SelectedItem = cbTargetCity.Items[0];
            }
            else
            {
                MessageBox.Show("Please enter a number!");
            }
        }

        private void buttFindPath_Click(object sender, RoutedEventArgs e)
        {
            if (cities.Count > 0)
            {
                List<int> shortestPath = FindShortestPath(cbFromCity.SelectedIndex, cbTargetCity.SelectedIndex);

                tbShortPath.Text = "Shortest path found: " + string.Join(",", shortestPath);
            }
            else
            {
                MessageBox.Show("Please build some cities first!");
            }
        }
    }
}