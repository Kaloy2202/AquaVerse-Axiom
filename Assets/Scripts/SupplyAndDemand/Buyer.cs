public class Buyer
{
    public string Name { get; private set; }
    public string Reason { get; private set; }
    public int Demand { get; private set; }
    public float Price { get; private set; }
    public float Timer { get; private set; }

    public Buyer(string name, string reason, int demand, float price, float timer)
    {
        Name = name;
        Reason = reason;
        Demand = demand;
        Price = price;
        Timer = timer;
    }

    public void UpdateTimer(float deltaTime)
    {
        Timer -= deltaTime;
    }

    public bool IsExpired()
    {
        return Timer <= 0;
    }
}
