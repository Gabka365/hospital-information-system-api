List<string> passes = new List<string>()
{
    { "TestAdminPassword" },
    { "TestPassword1Name" },
    { "TestPassword2Name" },
    { "TestPassword1Name" },
    { "TestPassword2Name" }
};


for (int i = 0; i < 5; i++)
{
    Console.WriteLine($"i: {i}");
    Console.WriteLine(Guid.NewGuid());
    Console.WriteLine(BCrypt.Net.BCrypt.HashPassword(passes[i]));
    Console.WriteLine();
}


