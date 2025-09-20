namespace Cadmus.Philology.Tools.Test;

public sealed class DeleteEditOperationTest
{
    [Fact]
    public void Execute_SingleChar()
    {
        DeleteEditOperation op = (DeleteEditOperation)
            EditOperation.ParseOperation("3!")!;
        const string a = "coniunx";

        string b = op.Execute(a);

        Assert.Equal("coiunx", b);
    }
}
