using System.Collections.Generic;
using Xunit;

namespace Cadmus.Philology.Tools.Test;

public class EditOperationTest
{
    [Fact]
    public void Diff_BothEmpty_ReturnsEmpty()
    {
        IList<EditOperation> ops = EditOperation.Diff("", "");
        Assert.Empty(ops);
    }

    [Fact]
    public void Diff_SourceEmpty_TargetNot_InsertAfter()
    {
        IList<EditOperation> ops = EditOperation.Diff("", "abc");
        Assert.Single(ops);
        InsertAfterEditOperation? op = ops[0] as InsertAfterEditOperation;
        Assert.NotNull(op);
        Assert.Equal(0, op.At);
        Assert.Equal("abc", op.Text);
    }

    [Fact]
    public void Diff_TargetEmpty_SourceNot_Delete()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "");
        Assert.Single(ops);
        DeleteEditOperation? op = ops[0] as DeleteEditOperation;
        Assert.NotNull(op);
        Assert.Equal(1, op.At);
        Assert.Equal(3, op.Run);
        Assert.Equal("abc", op.InputText);
    }

    [Fact]
    public void Diff_EqualStrings_NoOps()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "abc");
        Assert.Empty(ops);
    }

    [Fact]
    public void Diff_SingleReplace_ReplaceOp()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "adc");
        Assert.Single(ops);
        ReplaceEditOperation? op = ops[0] as ReplaceEditOperation;
        Assert.NotNull(op);
        Assert.Equal(2, op.At);
        Assert.Equal(1, op.Run);
        Assert.Equal("d", op.ReplacementText);
        Assert.Equal("b", op.InputText);
    }

    [Fact]
    public void Diff_InsertInMiddle_InsertBefore()
    {
        IList<EditOperation> ops = EditOperation.Diff("ac", "abc");
        Assert.Single(ops);
        InsertBeforeEditOperation? op = ops[0] as InsertBeforeEditOperation;
        Assert.NotNull(op);
        Assert.Equal(2, op.At);
        Assert.Equal("b", op.Text);
    }

    [Fact]
    public void Diff_DeleteInMiddle_DeleteOp()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "ac");
        Assert.Single(ops);
        DeleteEditOperation? op = ops[0] as DeleteEditOperation;
        Assert.NotNull(op);
        Assert.Equal(2, op.At);
        Assert.Equal(1, op.Run);
        Assert.Equal("b", op.InputText);
    }

    [Fact]
    public void Diff_MultipleEdits_MixedOps()
    {
        IList<EditOperation> ops = EditOperation.Diff("abcdef", "azced");
        Assert.Equal(3, ops.Count);

        // the expected operations are for:
        // abcdef
        // 123456
        // 1. replace 'b' with 'z' at position 2
        // 2. delete 'd' at position 4
        // 3. replace 'f' with 'd' at position 5
        Assert.IsType<ReplaceEditOperation>(ops[0]);
        ReplaceEditOperation? rep = ops[0] as ReplaceEditOperation;
        Assert.NotNull(rep);
        Assert.Equal("\"b\"@2=\"z\"", rep.ToString());

        Assert.IsType<DeleteEditOperation>(ops[1]);
        DeleteEditOperation? del = ops[1] as DeleteEditOperation;
        Assert.NotNull(del);
        Assert.Equal("\"d\"@4!", del.ToString());

        Assert.IsType<ReplaceEditOperation>(ops[2]);
        rep = ops[2] as ReplaceEditOperation;
        Assert.NotNull(rep);
        Assert.Equal("\"f\"@5=\"d\"", rep.ToString());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Diff_IncludeInputText_RespectsFlag(bool includeInputText)
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "adc",
            includeInputText);
        ReplaceEditOperation? op = ops[0] as ReplaceEditOperation;
        Assert.NotNull(op);
        if (includeInputText)
            Assert.Equal("b", op.InputText);
        else
            Assert.Null(op.InputText);
    }

    [Fact]
    public void Diff_InsertAtStart_InsertBefore()
    {
        IList<EditOperation> ops = EditOperation.Diff("bc", "abc");
        Assert.Single(ops);
        InsertBeforeEditOperation? op = ops[0] as InsertBeforeEditOperation;
        Assert.NotNull(op);
        Assert.Equal(1, op.At);
        Assert.Equal("a", op.Text);
    }

    [Fact]
    public void Diff_InsertAtEnd_InsertAfter()
    {
        IList<EditOperation> ops = EditOperation.Diff("ab", "abc");
        Assert.Single(ops);
        InsertAfterEditOperation? op = ops[0] as InsertAfterEditOperation;
        Assert.NotNull(op);
        Assert.Equal(2, op.At);
        Assert.Equal("c", op.Text);
    }

    [Fact]
    public void Diff_DeleteAtStart_DeleteOp()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "bc");
        Assert.Single(ops);
        DeleteEditOperation? op = ops[0] as DeleteEditOperation;
        Assert.NotNull(op);
        Assert.Equal(1, op.At);
        Assert.Equal(1, op.Run);
        Assert.Equal("a", op.InputText);
    }

    [Fact]
    public void Diff_DeleteAtEnd_DeleteOp()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "ab");
        Assert.Single(ops);
        DeleteEditOperation? op = ops[0] as DeleteEditOperation;
        Assert.NotNull(op);
        Assert.Equal(3, op.At);
        Assert.Equal(1, op.Run);
        Assert.Equal("c", op.InputText);
    }

    [Fact]
    public void Diff_ReplaceAtStart_ReplaceOp()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "xbc");
        Assert.Single(ops);
        ReplaceEditOperation? op = ops[0] as ReplaceEditOperation;
        Assert.NotNull(op);
        Assert.Equal(1, op.At);
        Assert.Equal("x", op.ReplacementText);
        Assert.Equal("a", op.InputText);
    }

    [Fact]
    public void Diff_ReplaceAtEnd_ReplaceOp()
    {
        IList<EditOperation> ops = EditOperation.Diff("abc", "abx");
        Assert.Single(ops);
        ReplaceEditOperation? op = ops[0] as ReplaceEditOperation;
        Assert.NotNull(op);
        Assert.Equal(3, op.At);
        Assert.Equal("x", op.ReplacementText);
        Assert.Equal("c", op.InputText);
    }
}
