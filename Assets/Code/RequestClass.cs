public class Request
{
    public string from_id;
    public string target_id;

    public Request(string from, string to)
    {
        this.from_id = from;
        this.target_id = to;
    }
}
