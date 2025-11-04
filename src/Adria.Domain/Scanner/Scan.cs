namespace Adria.Domain.Scanner;

public class Scan
{
    public Guid ScanId { get; private set; }
    public Guid AdrianId { get; private set; }
    public DateTime DateTime { get; private set; }
    public string Result { get; private set; }
    public string FoodId { get; private set; }
}