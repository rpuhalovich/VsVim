﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Microsoft.FSharp.Core;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using NUnit.Framework;
using Vim.Extensions;
using Vim.Modes;
using Vim.Modes.Command;

namespace Vim.UnitTest
{
    public static class Extensions
    {
        #region CountResult

        internal static CountResult.NeedMore AsNeedMore(this CountResult res)
        {
            return (CountResult.NeedMore)res;
        }

        #endregion

        #region ProcessResult

        public static ProcessResult.SwitchMode AsSwitchMode(this ProcessResult res)
        {
            return (ProcessResult.SwitchMode)res;
        }

        public static ProcessResult.SwitchModeWithArgument AsSwitchModeWithArgument(this ProcessResult res)
        {
            return (ProcessResult.SwitchModeWithArgument)res;
        }

        #endregion

        #region BindResult

        public static BindResult<T>.Complete AsComplete<T>(this BindResult<T> res)
        {
            Assert.IsTrue(res.IsComplete);
            return (BindResult<T>.Complete)res;
        }

        public static BindResult<T>.NeedMoreInput AsNeedMoreInput<T>(this BindResult<T> res)
        {
            Assert.IsTrue(res.IsNeedMoreInput);
            return (BindResult<T>.NeedMoreInput)res;
        }

        public static BindResult<TResult> Convert<T, TResult>(this BindResult<T> res, Func<T, TResult> func)
        {
            var func2 = func.ToFSharpFunc();
            return res.Convert(func2);
        }

        #endregion

        #region Command

        public static Command.LegacyCommand AsLegacyCommand(this Command command)
        {
            Assert.IsTrue(command.IsLegacyCommand);
            return (Command.LegacyCommand)command;
        }

        public static Command.VisualCommand AsVisualCommand(this Command command)
        {
            Assert.IsTrue(command.IsVisualCommand);
            return (Command.VisualCommand)command;
        }

        public static Command.NormalCommand AsNormalCommand(this Command command)
        {
            Assert.IsTrue(command.IsNormalCommand);
            return (Command.NormalCommand)command;
        }

        #endregion

        #region IMotionCapture

        public static BindResult<Tuple<Motion, FSharpOption<int>>> GetOperatorMotion(this IMotionCapture capture, char c)
        {
            return capture.GetOperatorMotion(KeyInputUtil.CharToKeyInput(c));
        }

        #endregion

        #region ModeUtil.Result

        public static Result.Failed AsFailed(this Result res)
        {
            return (Result.Failed)res;
        }

        #endregion

        #region ParseRangeResult

        internal static ParseRangeResult.Succeeded AsSucceeded(this ParseRangeResult res)
        {
            return (ParseRangeResult.Succeeded)res;
        }

        internal static ParseRangeResult.Failed AsFailed(this ParseRangeResult res)
        {
            return (ParseRangeResult.Failed)res;
        }

        #endregion

        #region IKeyMap

        public static IEnumerable<KeyInput> GetKeyMapping(this IKeyMap keyMap, char c, KeyRemapMode mode)
        {
            return GetKeyMapping(keyMap, KeyInputUtil.CharToKeyInput(c), mode);
        }

        public static IEnumerable<KeyInput> GetKeyMapping(this IKeyMap keyMap, string str, KeyRemapMode mode)
        {
            return GetKeyMapping(keyMap, KeyNotationUtil.StringToKeyInputSet(str), mode);
        }

        public static IEnumerable<KeyInput> GetKeyMapping(this IKeyMap keyMap, KeyInput ki, KeyRemapMode mode)
        {
            return GetKeyMapping(keyMap, KeyInputSet.NewOneKeyInput(ki), mode);
        }

        public static IEnumerable<KeyInput> GetKeyMapping(this IKeyMap keyMap, KeyInputSet kiSet, KeyRemapMode mode)
        {
            return keyMap.GetKeyMapping(kiSet, mode).AsMapped().Item.KeyInputs;
        }

        public static KeyMappingResult GetKeyMappingResult(this IKeyMap keyMap, KeyInput ki, KeyRemapMode mode)
        {
            return GetKeyMappingResult(keyMap, KeyInputSet.NewOneKeyInput(ki), mode);
        }

        public static KeyMappingResult GetKeyMappingResult(this IKeyMap keyMap, KeyInputSet set, KeyRemapMode mode)
        {
            return keyMap.GetKeyMapping(set, mode);
        }

        public static KeyMappingResult GetKeyMappingResult(this IKeyMap keyMap, char c, KeyRemapMode mode)
        {
            return GetKeyMappingResult(keyMap, KeyInputUtil.CharToKeyInput(c), mode);
        }

        public static KeyMappingResult GetKeyMappingResult(this IKeyMap keyMap, string str, KeyRemapMode mode)
        {
            return keyMap.GetKeyMapping(KeyNotationUtil.StringToKeyInputSet(str), mode);
        }

        #endregion

        #region KeyMappingResult

        public static KeyMappingResult.Mapped AsMapped(this KeyMappingResult res)
        {
            Assert.IsTrue(res.IsMapped);
            return (KeyMappingResult.Mapped)res;
        }

        public static KeyMappingResult.RecursiveMapping AsRecursiveMapping(this KeyMappingResult res)
        {
            Assert.IsTrue(res.IsRecursiveMapping);
            return (KeyMappingResult.RecursiveMapping)res;
        }

        #endregion

        #region SearchText

        public static SearchText.Pattern AsPattern(this SearchText text)
        {
            Assert.IsTrue(text.IsPattern);
            return (SearchText.Pattern)text;
        }

        public static SearchText.StraightText AsStraightText(this SearchText text)
        {
            Assert.IsTrue(text.IsStraightText);
            return (SearchText.StraightText)text;
        }

        public static SearchText.WholeWord AsWholeWord(this SearchText text)
        {
            Assert.IsTrue(text.IsWholeWord);
            return (SearchText.WholeWord)text;
        }

        #endregion

        #region SettingValue

        public static SettingValue.StringValue AsStringValue(this SettingValue value)
        {
            Assert.IsTrue(value.IsStringValue);
            return (SettingValue.StringValue)value;
        }

        public static SettingValue.ToggleValue AsToggleValue(this SettingValue value)
        {
            Assert.IsTrue(value.IsToggleValue);
            return (SettingValue.ToggleValue)value;
        }

        public static SettingValue.NumberValue AsNumberValue(this SettingValue value)
        {
            Assert.IsTrue(value.IsNumberValue);
            return (SettingValue.NumberValue)value;
        }

        #endregion

        #region RunResult

        public static RunResult.SubstituteConfirm AsSubstituteConfirm(this RunResult result)
        {
            Assert.IsTrue(result.IsSubstituteConfirm);
            return (RunResult.SubstituteConfirm)result;
        }

        #endregion

        #region IMode

        public static bool CanProcess(this IMode mode, VimKey key)
        {
            return mode.CanProcess(KeyInputUtil.VimKeyToKeyInput(key));
        }

        public static ProcessResult Process(this IMode mode, VimKey key)
        {
            return mode.Process(KeyInputUtil.VimKeyToKeyInput(key));
        }

        public static ProcessResult Process(this IMode mode, char c)
        {
            return mode.Process((KeyInputUtil.CharToKeyInput(c)));
        }

        public static ProcessResult Process(this IMode mode, string input, bool enter = false)
        {
            ProcessResult last = null;
            foreach (var c in input)
            {
                var i = KeyInputUtil.CharToKeyInput(c);
                last = mode.Process(c);
            }

            if (enter)
            {
                last = mode.Process(KeyInputUtil.EnterKey);
            }

            return last;
        }

        #endregion

        #region IVimBuffer

        public static bool Process(this IVimBuffer buf, VimKey key)
        {
            return buf.Process(KeyInputUtil.VimKeyToKeyInput(key));
        }

        public static bool Process(this IVimBuffer buf, char c)
        {
            return buf.Process(KeyInputUtil.CharToKeyInput(c));
        }

        public static void Process(this IVimBuffer buf, string input, bool enter = false)
        {
            foreach (var c in input)
            {
                var i = KeyInputUtil.CharToKeyInput(c);
                buf.Process(i);
            }
            if (enter)
            {
                buf.Process(KeyInputUtil.EnterKey);
            }
        }

        public static Register GetRegister(this IVimBuffer buffer, char c)
        {
            var name = RegisterNameUtil.CharToRegister(c).Value;
            return buffer.RegisterMap.GetRegister(name);
        }

        #endregion

        #region ITextView

        public static SnapshotPoint GetPoint(this ITextView textView, int position)
        {
            return new SnapshotPoint(textView.TextSnapshot, position);
        }

        public static SnapshotPoint GetEndPoint(this ITextView textView)
        {
            return textView.TextSnapshot.GetEndPoint();
        }

        public static ITextSnapshotLine GetLine(this ITextView textView, int line)
        {
            return textView.TextSnapshot.GetLineFromLineNumber(line);
        }

        public static SnapshotLineRange GetLineRange(this ITextView textView, int startLine, int endLine = -1)
        {
            endLine = endLine >= 0 ? endLine : startLine;
            return SnapshotLineRangeUtil.CreateForLineNumberRange(textView.TextSnapshot, startLine, endLine);
        }

        public static SnapshotSpan GetLineSpan(this ITextView textView, int lineNumber, int length)
        {
            return GetLineSpan(textView, lineNumber, 0, length);
        }

        public static SnapshotSpan GetLineSpan(this ITextView textView, int lineNumber, int column, int length)
        {
            return GetLineSpan(textView.TextBuffer, lineNumber, column, length);
        }

        public static ITextSnapshotLine GetLastLine(this ITextView textView)
        {
            return textView.TextSnapshot.GetLastLine();
        }

        public static ITextSnapshotLine GetFirstLine(this ITextView textView)
        {
            return textView.TextSnapshot.GetFirstLine();
        }

        public static CaretPosition MoveCaretTo(this ITextView textView, int position)
        {
            return textView.Caret.MoveTo(new SnapshotPoint(textView.TextSnapshot, position));
        }

        public static CaretPosition MoveCaretToLine(this ITextView textView, int lineNumber)
        {
            return MoveCaretTo(textView, textView.GetLine(lineNumber).Start.Position);
        }

        public static void SelectAndUpdateCaret(this ITextView textView, SnapshotSpan span, TextSelectionMode mode = TextSelectionMode.Stream)
        {
            textView.Selection.Mode = mode;
            textView.Selection.Select(span, false);
            MoveCaretTo(textView, span.End.Position);
        }

        public static ITextSnapshotLine GetCaretLine(this ITextView textView)
        {
            return textView.Caret.Position.BufferPosition.GetContainingLine();
        }

        public static void SetText(this ITextView textView, params string[] lines)
        {
            var text = lines.Aggregate((x, y) => x + y);
            SetText(textView.TextBuffer, text);
        }

        public static void SetText(this ITextView textView, string text, int? caret = null)
        {
            SetText(textView.TextBuffer, text);
            if (caret.HasValue)
            {
                MoveCaretTo(textView, caret.Value);
            }
        }

        public static VisualSpan GetVisualSpanBlock(this ITextView textView, int column, int length, int startLine = 0, int lineCount = 1)
        {
            return GetVisualSpanBlock(textView.TextBuffer, column, length, startLine, lineCount);
        }

        public static NonEmptyCollection<SnapshotSpan> GetBlock(this ITextView textView, int column, int length, int startLine = 0, int lineCount = 1)
        {
            return GetBlock(textView.TextBuffer, column, length, startLine, lineCount);
        }

        #endregion

        #region ITextBuffer

        public static ITextSnapshotLine GetLineFromLineNumber(this ITextBuffer buffer, int line)
        {
            return buffer.CurrentSnapshot.GetLineFromLineNumber(line);
        }

        public static ITextSnapshotLine GetLine(this ITextBuffer buffer, int line)
        {
            return buffer.CurrentSnapshot.GetLineFromLineNumber(line);
        }

        public static SnapshotLineRange GetLineRange(this ITextBuffer buffer, int startLine, int endLine = -1)
        {
            endLine = endLine >= 0 ? endLine : startLine;
            return SnapshotLineRangeUtil.CreateForLineNumberRange(buffer.CurrentSnapshot, startLine, endLine);
        }

        public static SnapshotSpan GetLineSpan(this ITextBuffer buffer, int lineNumber, int length)
        {
            return GetLineSpan(buffer, lineNumber, 0, length);
        }

        public static SnapshotSpan GetLineSpan(this ITextBuffer buffer, int lineNumber, int column, int length)
        {
            var line = buffer.GetLine(lineNumber);
            return new SnapshotSpan(line.Start.Add(column), length);
        }

        public static SnapshotPoint GetPoint(this ITextBuffer buffer, int position)
        {
            return new SnapshotPoint(buffer.CurrentSnapshot, position);
        }

        public static SnapshotPoint GetEndPoint(this ITextBuffer buffer)
        {
            return buffer.CurrentSnapshot.GetEndPoint();
        }

        public static SnapshotSpan GetExtent(this ITextBuffer buffer)
        {
            return buffer.CurrentSnapshot.GetExtent();
        }

        public static SnapshotSpan GetSpan(this ITextBuffer buffer, int start, int length)
        {
            return buffer.CurrentSnapshot.GetSpan(start, length);
        }

        public static void SetText(this ITextBuffer buffer, string text)
        {
            buffer.Replace(new Span(0, buffer.CurrentSnapshot.Length), text);
        }

        public static NonEmptyCollection<SnapshotSpan> GetBlock(this ITextBuffer textBuffer, int column, int length, int startLine = 0, int lineCount = 1)
        {
            var list = new List<SnapshotSpan>();
            for (var i = 0; i < lineCount; i++)
            {
                list.Add(textBuffer.GetLineSpan(i + startLine, column: column, length: length));
            }

            return NonEmptyCollectionUtil.OfSeq(list).Value;
        }

        public static VisualSpan GetVisualSpanBlock(this ITextBuffer textBuffer, int column, int length, int startLine = 0, int lineCount = 1)
        {
            var col = GetBlock(textBuffer, column, length, startLine, lineCount);
            return VisualSpan.NewBlock(col);
        }

        #endregion

        #region ITextSnapshot

        public static ITextSnapshotLine GetLine(this ITextSnapshot tss, int lineNumber)
        {
            return tss.GetLineFromLineNumber(lineNumber);
        }

        public static SnapshotLineRange GetLineRange(this ITextSnapshot tss, int startLine, int endLine = -1)
        {
            endLine = endLine >= 0 ? endLine : startLine;
            return SnapshotLineRangeUtil.CreateForLineNumberRange(tss, startLine, endLine);
        }

        public static ITextSnapshotLine GetFirstLine(this ITextSnapshot tss)
        {
            return GetLine(tss, 0);
        }

        public static ITextSnapshotLine GetLastLine(this ITextSnapshot tss)
        {
            return GetLine(tss, tss.LineCount - 1);
        }

        public static SnapshotPoint GetPoint(this ITextSnapshot tss, int position)
        {
            return new SnapshotPoint(tss, position);
        }

        public static SnapshotPoint GetEndPoint(this ITextSnapshot tss)
        {
            return new SnapshotPoint(tss, tss.Length);
        }

        public static SnapshotSpan GetSpan(this ITextSnapshot tss, int start, int length)
        {
            return new SnapshotSpan(tss, start, length);
        }

        public static SnapshotSpan GetExtent(this ITextSnapshot snapshot)
        {
            return new SnapshotSpan(snapshot, 0, snapshot.Length);
        }

        #endregion

        #region SnapshotPoint

        public static SnapshotSpan GetSpan(this SnapshotPoint point, int length)
        {
            return new SnapshotSpan(point, length);
        }

        #endregion

        #region SnapshotSpan

        /// <summary>
        /// Convert the SnapshotSpan into an EditSpan
        /// </summary>
        public static EditSpan ToEditSpan(this SnapshotSpan span)
        {
            return EditSpan.NewSingle(span);
        }

        #endregion

        #region VisualSpan

        public static VisualSpan.Character AsCharacter(this VisualSpan span)
        {
            Assert.IsTrue(span.IsCharacter);
            return (VisualSpan.Character)span;
        }

        public static VisualSpan.Line AsLine(this VisualSpan span)
        {
            Assert.IsTrue(span.IsLine);
            return (VisualSpan.Line)span;
        }

        public static VisualSpan.Block AsBlock(this VisualSpan span)
        {
            Assert.IsTrue(span.IsBlock);
            return (VisualSpan.Block)span;
        }

        #endregion

        #region ICommandRunner

        public static BindResult<CommandRunData> Run(this ICommandRunner runner, VimKey key)
        {
            return runner.Run(KeyInputUtil.VimKeyToKeyInput(key));
        }

        public static BindResult<CommandRunData> Run(this ICommandRunner runner, char c)
        {
            return runner.Run(KeyInputUtil.CharToKeyInput(c));
        }

        /// <summary>
        /// Run the multi-input command
        /// </summary>
        public static BindResult<CommandRunData> Run(this ICommandRunner runner, string command)
        {
            BindResult<CommandRunData> result = null;
            for (var i = 0; i < command.Length; i++)
            {
                result = runner.Run(command[i]);
                if (i + 1 < command.Length)
                {
                    Assert.IsTrue(result.IsNeedMoreInput);
                }
            }

            return result;
        }

        #endregion

        #region ICommonOperations

        public static void ShiftLineRangeLeft(this ICommonOperations operations, int count)
        {
            var number = operations.TextView.GetCaretLine().LineNumber;
            var range = operations.TextView.GetLineRange(number, number + (count - 1));
            operations.ShiftLineRangeLeft(range, 1);
        }

        public static void ShiftLineRangeRight(this ICommonOperations operations, int count)
        {
            var number = operations.TextView.GetCaretLine().LineNumber;
            var range = operations.TextView.GetLineRange(number, number + (count - 1));
            operations.ShiftLineRangeRight(range, 1);
        }

        #endregion

        #region Dispatcher

        public static void DoEvents(this System.Windows.Threading.Dispatcher dispatcher)
        {
            var frame = new DispatcherFrame();
            Action<DispatcherFrame> action = _ => { frame.Continue = false; };
            dispatcher.BeginInvoke(
                DispatcherPriority.SystemIdle,
                action,
                frame);
            Dispatcher.PushFrame(frame);
        }

        #endregion

        #region BindResult<T>

        public static BindResult<T> Run<T>(this BindResult<T> result, string text)
        {
            for (var i = 0; i < text.Length; i++)
            {
                var keyInput = KeyInputUtil.CharToKeyInput(text[i]);
                Assert.IsTrue(result.IsNeedMoreInput);
                result = result.AsNeedMoreInput().Item.BindFunction.Invoke(keyInput);
            }

            return result;
        }

        public static BindResult<T> Run<T>(this BindResult<T> result, VimKey vimKey)
        {
            Assert.IsTrue(result.IsNeedMoreInput);
            return result.AsNeedMoreInput().Item.Run(vimKey);
        }

        #endregion

        #region BindData<T>

        public static BindResult<T> Run<T>(this BindData<T> data, string text)
        {
            var keyInput = KeyInputUtil.CharToKeyInput(text[0]);
            return data.BindFunction.Invoke(keyInput).Run(text.Substring(1));
        }

        public static BindResult<T> Run<T>(this BindData<T> data, VimKey vimKey)
        {
            var keyInput = KeyInputUtil.VimKeyToKeyInput(vimKey);
            return data.BindFunction.Invoke(keyInput);
        }

        #endregion

        #region SearchResult

        public static SearchResult.SearchFound AsSearchFound(this SearchResult result)
        {
            Assert.IsTrue(result.IsSearchFound);
            return (SearchResult.SearchFound)result;
        }

        #endregion


        #region IIncrementalSearch

        public static BindResult<SearchResult> DoSearch(this IIncrementalSearch search, string text, SearchKind searchKind = SearchKind.ForwardWithWrap)
        {
            return search.Begin(searchKind).Run(text).Run(VimKey.Enter);
        }

        #endregion

        public static SnapshotSpan GetSpan(this ITextSelection selection)
        {
            var span = new SnapshotSpan(selection.Start.Position, selection.End.Position);
            return span;
        }

        public static void Select(this ITextSelection selection, SnapshotPoint startPoint, SnapshotPoint endPoint)
        {
            selection.Select(new VirtualSnapshotPoint(startPoint), new VirtualSnapshotPoint(endPoint));
        }

        public static void Select(this ITextSelection selection, params SnapshotSpan[] spans)
        {
            if (spans.Length == 1)
            {
                selection.Mode = TextSelectionMode.Stream;
                selection.Clear();
                selection.Select(spans[0], false);
            }
            else
            {
                selection.Mode = TextSelectionMode.Box;
                foreach (var span in spans)
                {
                    selection.Select(span, false);
                }
            }
        }

        public static void UpdateValue(this Register reg, string value)
        {
            UpdateValue(reg, value, OperationKind.CharacterWise);
        }

        public static void UpdateValue(this Register reg, string value, OperationKind kind)
        {
            var data = StringData.NewSimple(value);
            var regValue = new RegisterValue(data, kind);
            reg.Value = regValue;
        }

        public static void UpdateBlockValues(this Register reg, params string[] value)
        {
            var data = StringData.NewBlock(value.ToFSharpList());
            reg.Value = new RegisterValue(data, OperationKind.CharacterWise);
        }

        public static SnapshotPoint GetCaretPoint(this ITextView view)
        {
            return view.Caret.Position.BufferPosition;
        }

        public static VirtualSnapshotPoint GetCaretVirtualPoint(this ITextView view)
        {
            return view.Caret.Position.VirtualBufferPosition;
        }

        public static SnapshotSpan GetSelectionSpan(this ITextView textView)
        {
            return textView.Selection.StreamSelectionSpan.SnapshotSpan;
        }

        public static Register GetRegister(this IRegisterMap map, char c)
        {
            var name = RegisterNameUtil.CharToRegister(c).Value;
            return map.GetRegister(name);
        }

        public static bool IsSome<T>(this FSharpOption<T> option, T value)
        {
            Assert.IsTrue(option.IsSome(), "Option is None");
            Assert.AreEqual(value, option.Value);
            return true;
        }

        public static bool IsSome<T>(this FSharpOption<T> option, Func<T, bool> func)
        {
            Assert.IsTrue(option.IsSome());
            Assert.IsTrue(func(option.Value));
            return true;
        }
    }
}
