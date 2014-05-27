/**********************************************************
 * Name:        ExcelWrapper
 * Author:      Tim Claason
 * Date:        November 17, 2011
 * Description: Provides a fast simple front-end to the
 *              C#-based NPOI library for interacting with
 *              Excel files
 * Notes:	Updated on April 3, 2012 to fix FromDataTable()
 *		- it was not outputting column headers
 **********************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using NPOI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Magnolia.FileUtilities
{
    public class ExcelWrapper
    {
        /// <summary>
        /// The Excel file
        /// </summary>
        HSSFWorkbook _workBook;

        /// <summary>
        /// The worksheet within the workbook
        /// </summary>
        ISheet _workSheet;

        /// <summary>
        /// Location of the excel spreadsheet
        /// </summary>
        string _pathAndFile;

        /// <summary>
        /// Number of used rows in the current spreadsheet
        /// </summary>
        private int _rowCountInSpreadsheet;

        /// <summary>
        /// Number of used columns in the current spreadsheet
        /// </summary>
        private int _columnCountInSpreadsheet;

        /// <summary>
        /// Represents any exceptions that occured - for use in logging
        /// </summary>
        private string _importResultsString = String.Empty;

        /// <summary>
        /// Current row in the sheet
        /// </summary>
        private IRow _currentRow;

        /// <summary>
        /// Current cell in the sheet (and row)
        /// </summary>
        private ExcelCell _currentCell;

        /// <summary>
        /// Underlying file stream
        /// </summary>
        FileStream _underlyingStream;

        /// <summary>
        /// Maximum number of rows in the sheet
        /// </summary>
        const int MAXIMUM_ROWS = 65500;

        /// <summary>
        /// Maximum length of sheet
        /// </summary>
        const int MAXIMUM_SHEET_NAME_LENGTH = 25;

        /// <summary>
        /// Default Constructor - For Creating a new workbook; Use in conjunction with CreateFile()
        /// </summary>
        public ExcelWrapper()
        {

        }

        public ExcelWrapper(string pathAndFile)
        {
            _pathAndFile = pathAndFile;
            openWorkbook();
        }

        /// <summary>
        /// Opens the workbook for reading
        /// </summary>
        private void openWorkbook()
        {
            if (!File.Exists(_pathAndFile))
            {
                this.CreateFile(_pathAndFile);
            }

            _underlyingStream = new FileStream(_pathAndFile, FileMode.Open, FileAccess.ReadWrite);

            _workBook = new HSSFWorkbook(_underlyingStream);

            if (_workBook.NumberOfSheets > 0)
                _workSheet = _workBook.GetSheetAt(0);

            _underlyingStream.Close();
        }

        private void initializeCell(int row, int column)
        {
            _currentRow = _workSheet.GetRow(row);
            _currentCell = new ExcelCell(_workBook, _workSheet, row, column);
        }


        /// <summary>
        /// Sets the active worksheet to the default sheet in the workbook
        /// </summary>
        private void setActiveSheet()
        {
            this.SetActiveSheet(0);
        }


        /// <summary>
        /// Creates a file in the specified location
        /// </summary>
        /// <param name="pathAndFile">Location to create the file</param>
        public void CreateFile(string pathAndFile)
        {
            _pathAndFile = pathAndFile;
            _workBook = new HSSFWorkbook();
            _workSheet = _workBook.CreateSheet("Sheet1");

            _underlyingStream = new FileStream(pathAndFile, FileMode.Create);
            _workBook.Write(_underlyingStream);
            _underlyingStream.Close();
        }

        /// <summary>
        /// Public access to the xls file that you are interacting with
        /// </summary>
        public string PathAndFile
        {
            get { return _pathAndFile; }
            set { _pathAndFile = value; }
        }

        /// <summary>
        /// Number of worksheets in the workbook
        /// </summary>
        public int WorksheetCount
        {
            get
            {
                if (_workBook == null)
                    return -1;
                return _workBook.NumberOfSheets;
            }
        }

        /// <summary>
        /// Checks to see if a sheet exists
        /// </summary>
        /// <param name="sheetName">Name of worksheet</param>
        /// <returns>True if the sheet exists; otherwise false</returns>
        public bool SheetExists(string sheetName)
        {
            try
            {
                ISheet s = _workBook.GetSheet(sheetName);
                return s != null;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Writes a value into a cell
        /// </summary>
        /// <param name="row">Row number (0-based)</param>
        /// <param name="column">Column number (0-based)</param>
        /// <param name="value">String value to write</param>
        /// <param name="isBold">Bold flag</param>
        /// <param name="isItalics">Italics flag</param>
        public void WriteCellValue(int row, int column, string value, bool isBold, bool isItalics)
        {
            this.initializeCell(row, column);
            _currentCell.WriteCellValue(value, isBold, isItalics);
        }

        /// <summary>
        /// Writes a value to a cell with the given style
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        /// <param name="value">Value to write to cell</param>
        /// <param name="style">Style</param>
        public void WriteCellValue(int row, int column, string value, ExcelStyle style)
        {
            this.initializeCell(row, column);
            _currentCell.Style = style;
            _currentCell.WriteCellValue(value);
        }

        /// <summary>
        /// Writes a value to a cell - 0 based
        /// </summary>
        /// <param name="value">Value to write to the cell</param>
        /// <param name="row">Row number</param>
        /// <param name="column">Column Number</param>
        public void WriteCellValue(int row, int column, string value)
        {
            try
            {
                this.WriteCellValue(row, column, value, false, false);
            }
            catch { }
        }

        /// <summary>
        /// Writes a value to a cell - based on the Excel cell name (A1, AZ23, etc)
        /// </summary>
        /// <param name="excelCellName">Excel cell name</param>
        /// <param name="value">Value to write</param>
        public void WriteCellValue(string excelCellName, string value)
        {
            try
            {
                ExcelCell cell = new ExcelCell(_workBook, _workSheet, excelCellName);
                _currentCell = cell;
                _currentCell.WriteCellValue(value);
            }
            catch { }

        }

        /// <summary>
        /// Saves the excel file to the disk
        /// </summary>
        public void SaveFile()
        {
            this.SaveFileAs(_pathAndFile);
        }

        /// <summary>
        /// Saves the excel file to the disk
        /// </summary>
        public void SaveFileAs(string pathAndFile)
        {            
            _underlyingStream = new FileStream(pathAndFile, FileMode.OpenOrCreate);
            _workBook.Write(_underlyingStream);
            _underlyingStream.Close();
        }

        /// <summary>
        /// Closes workbook and releases resources
        /// </summary>
        public void CloseWorkBook()
        {
            try
            {
                _underlyingStream.Close();
                //_workBook.Dispose();
            }
            catch { }
        }


        /// <summary>
        /// Sets a 0-based sheet index
        /// </summary>
        /// <param name="index">0-based sheet index number</param>
        public void SetActiveSheet(int index)
        {
            if (this.WorksheetCount >= index)
                _workSheet = _workBook.GetSheetAt(index);
        }

        /// <summary>
        /// Sets the current sheet to the specified sheet name
        /// </summary>
        /// <param name="sheetName">Name of sheet</param>
        public void SetActiveSheet(string sheetName)
        {
            try
            {
                if (!this.SheetExists(sheetName))
                {
                    this.CreateSheet(sheetName);
                    return;
                }

                for (int i = 0; i < _workBook.NumberOfSheets; i++)
                {
                    ISheet s = _workBook.GetSheetAt(i);
                    if (s.SheetName.ToUpper().Trim() == sheetName.ToUpper().Trim())
                    {
                        _workSheet = s;
                        //_workBook.ActiveSheetIndex = i;
                        return;
                    }
                }

                _workSheet = _workBook.GetSheet(sheetName);

            }
            catch
            {
                this.setActiveSheet();
            }
        }

        /// <summary>
        /// Deletes a sheet from the workbook if it exists
        /// </summary>
        /// <param name="sheetName">Name of sheet to delete</param>
        public void DeleteSheet(string sheetName)
        {
            if (!this.SheetExists(sheetName))
                return;

            for (int i = 0; i < _workBook.NumberOfSheets; i++)
            {
                ISheet s = _workBook.GetSheetAt(i);
                if (s.SheetName.ToUpper() == sheetName.ToUpper())
                {
                    _workBook.RemoveSheetAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Attempts to create a sheet - if it doesn't already exist
        /// </summary>
        /// <param name="sheetName">Name of sheet to create</param>
        public void CreateSheet(string sheetName)
        {
            if (sheetName.Length > MAXIMUM_SHEET_NAME_LENGTH)
                throw new Exception("Error - cannot create sheet " + sheetName + " because the length of a sheet name cannot exceed " + MAXIMUM_SHEET_NAME_LENGTH + " characters");
            if (this.SheetExists(sheetName))
                throw new Exception("Error - sheet " + sheetName + " exists");

            _workSheet = _workBook.CreateSheet(sheetName);
        }

        /// <summary>
        /// Highlights the background color of the cell
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        /// <param name="color">Excel color</param>
        public void HighlightCell(int row, int column, ExcelColors color)
        {
            this.initializeCell(row, column);
            _currentCell.Style.BackColor = color;
            _currentCell.WriteCellValue();
        }

        /// <summary>
        /// Sets the font color of a cell
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        /// <param name="color">Excel Color</param>
        public void SetCellFontColor(int row, int column, ExcelColors color)
        {
            this.initializeCell(row, column);
            _currentCell.Style.ForeColor = color;
            _currentCell.WriteCellValue();
        }

        /// <summary>
        /// Sets the border on the Excel cell - does not support piece-meal borders, even though NPOI does
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        /// <param name="borderType">Type of border</param>
        public void SetBorder(int row, int column, BorderTypes borderType)
        {
            this.initializeCell(row, column);
            _currentCell.Style.BorderType = borderType;

            if (borderType != BorderTypes.None)
            {
                _currentCell.Style.BorderBottom = true;
                _currentCell.Style.BorderLeft = true;
                _currentCell.Style.BorderRight = true;
                _currentCell.Style.BorderTop = true;
            }
            _currentCell.WriteCellValue();
        }

        /// <summary>
        /// Gets the cell value of the cell
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        /// <returns>String representation of cell value</returns>
        public string GetCellValue(int row, int column)
        {
            if (_workBook == null)
                throw new Exception("Error - workbook is not initialized");
            if (_workSheet == null)
                throw new Exception("Error - worksheet is not initialized");

            try
            {
                this.initializeCell(row, column);
                return _currentCell.Value;
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Gets a cell's value
        /// </summary>
        /// <param name="cellName">Excel cell name (A1, B32, etc)</param>
        /// <returns>String representation of the cell's value</returns>
        public string GetCellValue(string cellName)
        {
            ExcelCell cell = new ExcelCell(_workBook, _workSheet, cellName);
            if (!cell.IsValidCell)
                throw new Exception("Error - referenced cell name is not a valid cell");
            return this.GetCellValue(cell.Row, cell.Column);
        }

        /// <summary>
        /// Gets the name of the current sheet
        /// </summary>
        /// <returns>String representation of the current sheet name</returns>
        public string GetCurrentSheet()
        {
            if (_workSheet == null)
                return String.Empty;
            return _workSheet.SheetName;
        }

        /// <summary>
        /// Approximates the number of rows in the spreadsheet
        /// </summary>
        /// <returns>Approximate number of rows in the spreadsheet</returns>
        public int GetNumberOfRows()
        {
            if (_workSheet == null)
                return 0;
            _rowCountInSpreadsheet = _workSheet.LastRowNum;
            return _rowCountInSpreadsheet;
        }

        /// <summary>
        /// Changs a cell's alignment        
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        /// <param name="style">Easy to use enum for external use</param>
        public void AlignCell(int row, int column, AlignStyles style)
        {
            initializeCell(row, column);
            _currentCell.Style.Alignment = style;
            _currentCell.WriteCellValue(_currentCell.Value);
        }

        /// <summary>
        /// Gets the number of columns in the current sheet
        /// </summary>
        /// <returns>Number of columns in the spreadsheet</returns>
        public int GetNumberOfColumns()
        {
            if (_workSheet == null)
                return 0;

            this.initializeCell(0, 0);
            _columnCountInSpreadsheet = _currentRow.PhysicalNumberOfCells;
            return _columnCountInSpreadsheet;
        }

        /// <summary>
        /// Gets a datatable representation of an excel spreadsheet range.  Make sure the first row has column names.
        /// Use with the constructor that specifies file path
        /// </summary>
        /// <returns>A datatable of the current spreadsheet</returns>
        public System.Data.DataTable ToDataTable()
        {
            if (_workBook == null || _workSheet == null)
                return new DataTable();

            System.Data.DataTable returnDataTable = new System.Data.DataTable();
            int columnIndex = 0;

            for (int i = 0; i <= this.GetNumberOfColumns(); i++)
            {
                string columnName = GetCellValue(0, i);
                if (columnName == String.Empty)
                    continue;

                if (returnDataTable.Columns.Contains(columnName))
                    continue;
                returnDataTable.Columns.Add(columnName);
            }

            for (int row = 0; row <= this.GetNumberOfRows(); row++)
            {
                if (row == 0)
                    continue;

                columnIndex = 0;

                System.Data.DataRow newRow = returnDataTable.NewRow();

                for (int column = 0; column <= this.GetNumberOfColumns(); column++)
                {
                    string cellValue = GetCellValue(row, column);

                    if (returnDataTable.Columns.Count > columnIndex + 1)
                        newRow[columnIndex] = cellValue;
                    columnIndex++;
                }
                returnDataTable.Rows.Add(newRow);

            }

            return returnDataTable;

        }

        /// <summary>
        /// Populates the active sheet with the contents of the input datatable - will not erase previously set cells if that cell is outside the parameters of the datatable size
        /// </summary>
        /// <param name="inputTable">System.Data.DataTable</param>
        public void FromDataTable(DataTable inputTable)
        {
            if (inputTable == null || inputTable.Columns.Count == 0)
                return;

            for (int i = 0; i < inputTable.Columns.Count; i++)
            {
                DataColumn c = inputTable.Columns[i];
                this.WriteCellValue(0, i, c.ColumnName);
            }

            for (int j = 0; j < inputTable.Rows.Count; j++)
            {   
                for (int k = 0; k < inputTable.Columns.Count; k++)
                {
                    string currentValue = inputTable.Rows[j][k].ToString();
                    this.WriteCellValue(j + 1, k, currentValue);
                }

                if (j >= MAXIMUM_ROWS)
                    break;
            }
        }

        /// <summary>
        /// Converts a DataTable to Excel Output, and allows for specification of output sheet
        /// </summary>
        /// <param name="inputTable">DataTable to output</param>
        /// <param name="outputSheet">Sheet to put the output</param>
        public void FromDataTable(DataTable inputTable, string outputSheet)
        {
            this.SetActiveSheet(outputSheet);
            this.FromDataTable(inputTable);
        }

        /// <summary>
        /// Gets the workbook represented as a byte collection
        /// </summary>
        /// <returns>Byte array</returns>
        public byte[] GetBytes()
        {
            if (_workBook == null)
                throw new Exception("Error - workbook is null");

            using (var buffer = new MemoryStream())
            {
                _workBook.Write(buffer);
                return buffer.GetBuffer();
            }
        }

        /// <summary>
        /// Auto-sizes column
        /// </summary>
        /// <param name="column">0-based column</param>
        public void AutoSizeColumn(int column)
        {
            _workSheet.AutoSizeColumn(column);
        }

        /// <summary>
        /// Specifies width for column
        /// </summary>
        /// <param name="column">0-based column</param>
        public void SetColumnWidth(int column, int width)
        {
            _workSheet.SetColumnWidth(column, width);
        }
    }

    /// <summary>
    /// Factory for converting Wrapper alignment to NPOI alignment and border style
    /// </summary>
    public class CellStyleFactory
    {
        /// <summary>
        /// Converts Wrapper Alignment to NPOI alignment
        /// </summary>
        /// <param name="style">Wrapper style</param>
        /// <returns>NPOI style</returns>
        public static HorizontalAlignment GetAlignment(AlignStyles style)
        {
            if (style == AlignStyles.Left)
                return HorizontalAlignment.LEFT;
            else if (style == AlignStyles.Center)
                return HorizontalAlignment.CENTER;
            else if (style == AlignStyles.Right)
                return HorizontalAlignment.RIGHT;
            else if (style == AlignStyles.Justify)
                return HorizontalAlignment.JUSTIFY;
            return HorizontalAlignment.LEFT;
        }


        /// <summary>
        /// Translates a known ExcelColor into the [short] HSSF version - allows a limited subset of colors
        /// </summary>
        /// <param name="color">Color to translate</param>
        /// <returns>A short representation of the color</returns>
        public static short GetColor(ExcelColors color)
        {
            if (color == ExcelColors.Black)
                return NPOI.HSSF.Util.HSSFColor.BLACK.index;
            else if (color == ExcelColors.Blue)
                return NPOI.HSSF.Util.HSSFColor.BLUE.index;
            else if (color == ExcelColors.Green)
                return NPOI.HSSF.Util.HSSFColor.GREEN.index;
            else if (color == ExcelColors.Orange)
                return NPOI.HSSF.Util.HSSFColor.ORANGE.index;
            else if (color == ExcelColors.Red)
                return NPOI.HSSF.Util.HSSFColor.RED.index;
            else if (color == ExcelColors.White)
                return NPOI.HSSF.Util.HSSFColor.WHITE.index;
            else if (color == ExcelColors.Yellow)
                return NPOI.HSSF.Util.HSSFColor.YELLOW.index;
            else if (color == ExcelColors.LightBlue)
                return NPOI.HSSF.Util.HSSFColor.LIGHT_BLUE.index;
            else if (color == ExcelColors.LightGreen)
                return NPOI.HSSF.Util.HSSFColor.LIGHT_GREEN.index;
            else if (color == ExcelColors.LightYellow)
                return NPOI.HSSF.Util.HSSFColor.LIGHT_YELLOW.index;
            return NPOI.HSSF.Util.HSSFColor.AUTOMATIC.index;

        }
    }

    /// <summary>
    /// A utility class for translating Excel cell row and columns from and to numbers and cell names, and for manipulating underling NPOI cells
    /// </summary>
    public class ExcelCell
    {
        /// <summary>
        /// Underlying NPOI cell
        /// </summary>
        private ICell _cell;

        /// <summary>
        /// Style associated with the cell
        /// </summary>
        private ExcelStyle _style;

        /// <summary>
        /// Current row in the sheet
        /// </summary>
        private IRow _currentRow;

        /// <summary>
        /// Maximum number of rows in the sheet
        /// </summary>
        const int MAXIMUM_ROWS = 65500;

        /// <summary>
        /// Worksheet that the cell is on
        /// </summary>
        private ISheet _workSheet;

        /// <summary>
        /// Workbook that the cell is in
        /// </summary>
        private IWorkbook _workBook;

        /// <summary>
        /// Constructor for accepting a typical Excel Cell name - A1, AZ23, B72, etc
        /// </summary>
        /// <param name="worksheet">Worksheet the cell is on</param>
        /// <param name="excelName">Excel Cell Name</param>
        public ExcelCell(HSSFWorkbook workbook, ISheet worksheet, string excelName)
        {
            _workBook = workbook;
            _style = new ExcelStyle();
            _workSheet = worksheet;
            this.ExcelName = excelName;
            this.parseExcelName();
            this.initializeCell(this.Row, this.Column);
        }

        /// <summary>
        /// Constructor for accepting a 0-based row and cell coordinate, and converting to an Excel Cell name
        /// </summary>
        /// <param name="worksheet">Worksheet the cell is on</param>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        public ExcelCell(HSSFWorkbook workbook, ISheet worksheet, int row, int column)
        {
            _workBook = workbook;
            _style = new ExcelStyle();
            _workSheet = worksheet;
            this.Row = row;
            this.Column = column;
            this.parseNumericCellComponents();
            this.initializeCell(row, column);
        }

        /// <summary>
        /// Initializes a cell, prepares it for reading and writing
        /// </summary>
        /// <param name="row">0-based row number</param>
        /// <param name="column">0-based column number</param>
        private void initializeCell(int row, int column)
        {
            if (row > MAXIMUM_ROWS)
                throw new Exception("Cannot initialize cell " + row + " because it exceeds the system maximum");

            _currentRow = _workSheet.GetRow(row);

            if (_currentRow == null)
            {
                _currentRow = _workSheet.CreateRow(row);
            }

            _cell = _currentRow.GetCell(column);

            if (_cell == null)
            {
                _cell = _currentRow.CreateCell(column);
            }
        }

        /// <summary>
        /// Converts a row/column combination into an Excel Cell Name
        /// </summary>
        private void parseNumericCellComponents()
        {
            string[] columns = this.getColumnNameCollection();
            if (this.Column < columns.Length)
                this.ExcelName = columns[this.Column] + this.Row.ToString();
            else
                this.ExcelName = "A" + this.Row.ToString();
        }

        /// <summary>
        /// Converts an Excel Cell Name into a 0-based Row and Column coordinate
        /// </summary>
        private void parseExcelName()
        {
            char[] charactersInValue = this.ExcelName.ToCharArray();
            string letterComponent = String.Empty;
            string numberComponent = String.Empty;
            int letterAsColumnNumber = 0;
            foreach (char c in charactersInValue)
            {
                int number;
                bool parsedAsInt = int.TryParse(c.ToString(), out number);

                if (parsedAsInt)
                    numberComponent += c.ToString();
                else
                    letterComponent += c.ToString();
            }

            int columnIndex = 0;
            foreach (string columnLetter in this.getColumnNameCollection())
            {
                if (columnLetter.ToUpper() == letterComponent.ToUpper())
                {
                    letterAsColumnNumber = columnIndex;
                    break;
                }
                columnIndex++;
            }

            int outputNumber;
            bool parsed = int.TryParse(numberComponent, out outputNumber);
            if (parsed)
                this.Row = outputNumber - 1;
            this.Column = letterAsColumnNumber;

        }

        /// <summary>
        /// Public access to the Excel name
        /// </summary>
        public string ExcelName
        {
            get;
            set;
        }

        /// <summary>
        /// Public access to the 0-based row number
        /// </summary>
        public int Row
        {
            get;
            set;
        }

        /// <summary>
        /// Public access to the 0-based column number
        /// </summary>
        public int Column
        {
            get;
            set;
        }

        /// <summary>
        /// Text value of this cell
        /// </summary>
        public string Value
        {
            get
            {
                try
                {
                    this.initializeCell(this.Row, this.Column);

                    if (_cell.CellType == CellType.STRING || _cell.CellType == CellType.FORMULA)
                        return _cell.StringCellValue;
                    else if (_cell.CellType == CellType.BOOLEAN)
                        return _cell.BooleanCellValue.ToString();
                    else if (_cell.CellType == CellType.NUMERIC)
                        return _cell.NumericCellValue.ToString();
                    else
                        return String.Empty;
                }
                catch
                {
                    return String.Empty;
                }
            }
        }

        /// <summary>
        /// Good-faith estimate of whether or not the passed parameters translate to a valid Excel Cell
        /// </summary>
        public bool IsValidCell
        {
            get
            {
                return this.Row >= 0 && this.Column >= 0 && this.ExcelName != null && this.ExcelName.Length > 0;
            }
        }

        /// <summary>
        /// The style associated with the cell
        /// </summary>
        public ExcelStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
            }
        }

        /// <summary>
        /// Gets a collection of known Excel column names
        /// </summary>
        /// <returns>A string array of column names</returns>
        private string[] getColumnNameCollection()
        {
            return new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", 
                                "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ",
                                "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH", "BI", "BJ", "BK", "BL", "BM", "BN", "BO", "BP", "BQ", "BR", "BS", "BT", "BU", "BV", "BW", "BX", "BY", "BZ",
                                "CA", "CB", "CC", "CD", "CE", "CF", "CG", "CH", "CI", "CJ", "CK", "CL", "CM", "CN", "CO", "CP", "CQ", "CR", "CS", "CT", "CU", "CV", "CW", "CX", "CY", "CZ",
                                "DA", "DB", "DC", "DD", "DE", "DF", "DG", "DH", "DI", "DJ", "DK", "DL", "DM", "DN", "DO", "DP", "DQ", "DR", "DS", "DT", "DU", "DV", "DW", "DX", "DY", "DZ", 
                                "EA", "EB", "EC", "ED", "EE", "EF", "EG", "EH", "EI", "EJ", "EK", "EL", "EM", "EN", "EO", "EP", "EQ", "ER", "ES", "ET", "EU", "EV", "EW", "EX", "EY", "EZ",
                                "FA", "FB", "FC", "FD", "FE", "FF", "FG", "FH", "FI", "FJ", "FK", "FL", "FM", "FN", "FO", "FP", "FQ", "FR", "FS", "FT", "FU", "FV", "FW", "FX", "FY", "FZ",
                                "GA", "GB", "GC", "GD", "GE", "GF", "GG", "GH", "GI", "GJ", "GK", "GL", "GM", "GN", "GO", "GP", "GQ", "GR", "GS", "GT", "GU", "GV", "GW", "GX", "GY", "GZ",
                                "HA", "HB", "HC", "HD", "HE", "HF", "HG", "HH", "HI", "HJ", "HK", "HL", "HM", "HN", "HO", "HP", "HQ", "HR", "HS", "HT", "HU", "HV", "HW", "HX", "HY", "HZ",
                                "IA", "IB", "IC", "ID", "IE", "IF", "IG", "IH", "II", "IJ", "IK", "IL", "IM", "IN", "IO", "IP", "IQ", "IR", "IS", "IT", "IU", "IV", "IW", "IX", "IY", "IZ" };
        }

        /// <summary>
        /// Writes a value into a cell
        /// </summary>
        /// <param name="row">Row number (0-based)</param>
        /// <param name="column">Column number (0-based)</param>
        /// <param name="value">String value to write</param>
        /// <param name="isBold">Bold flag</param>
        /// <param name="isItalics">Italics flag</param>
        public void WriteCellValue(string value, bool isBold, bool isItalics)
        {
            _style.IsBold = isBold;
            _style.IsItalics = isItalics;
            this.WriteCellValue(value);
        }

        /// <summary>
        /// Writes value to this cell
        /// </summary>
        /// <param name="value">Value to write to cell</param>
        public void WriteCellValue(string value)
        {
            this.initializeCell(this.Row, this.Column);

            if (value == String.Empty)
            {
                _currentRow.RemoveCell(_cell);
                return;
            }

            _cell.SetCellValue(value);
        }

        /// <summary>
        /// Writes the current value of the cell to the cell again, presumable for use with updated style
        /// </summary>
        public void WriteCellValue()
        {
            this.WriteCellValue(this.Value);
        }

    }

    /// <summary>
    /// A quick way to wrap (some) of the colors exposed by the NPOI library
    /// </summary>
    public enum ExcelColors
    {
        Yellow,
        Red,
        Blue,
        Green,
        Black,
        White,
        Orange,
        LightBlue,
        LightGreen,
        LightYellow
    }

    /// <summary>
    /// Allowed cell alignments
    /// </summary>
    public enum AlignStyles
    {
        Left,
        Center,
        Right,
        Justify
    }

    /// <summary>
    /// Allowed border types
    /// </summary>
    public enum BorderTypes
    {
        Dashed,
        Dotted,
        Double,
        Hair,
        Thin,
        Medium,
        Thick,
        None
    }

    /// <summary>
    /// Common fonts
    /// </summary>
    public enum CommonFonts
    {
        Arial,
        Times,
        Courier,
        Tahoma,
        Calibri,
        Batang,
        Broadway,
        Cambria,
        Castellar,
        Century,
        Fixedsys,
        Garamond,
        Georgia,
        Harrington,
        Terminal,
        Wingdings

    }

    /// <summary>
    /// Data structure for storing information about a Cell Style
    /// </summary>
    public class ExcelStyle
    {
        /// <summary>
        /// Maximum possible font size
        /// </summary>
        const int MAXIMUM_FONT_SIZE = 409;

        /// <summary>
        /// Private storage of font size
        /// </summary>
        private int _fontSize;

        /// <summary>
        /// Default constructor of ExcelStyle
        /// </summary>
        public ExcelStyle()
        {
            this.IsBold = false;
            this.IsItalics = false;
            this.IsUnderlined = false;
            this._fontSize = 10;
            this.FontFace = CommonFonts.Arial;
            this.BorderType = BorderTypes.None;
            this.Alignment = AlignStyles.Left;
            this.BorderBottom = false;
            this.BorderLeft = false;
            this.BorderRight = false;
            this.BorderTop = false;
            this.BackColor = ExcelColors.White;
            this.ForeColor = ExcelColors.Black;
            this.WrapText = false;
        }

        public static ExcelStyle GetGenericStyle()
        {
            return new ExcelStyle();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ExcelStyle))
                return false;
            ExcelStyle passedStyle = (ExcelStyle)obj;

            return this.IsBold == passedStyle.IsBold &&
                this.Alignment == passedStyle.Alignment &&
                this.BackColor == passedStyle.BackColor &&
                this.BorderBottom == passedStyle.BorderBottom &&
                this.BorderLeft == passedStyle.BorderLeft &&
                this.BorderRight == passedStyle.BorderRight &&
                this.BorderTop == passedStyle.BorderTop &&
                this.BorderType == passedStyle.BorderType &&
                this.FontFace == passedStyle.FontFace &&
                this.FontSize == passedStyle.FontSize &&
                this.ForeColor == passedStyle.ForeColor &&
                this.IsItalics == passedStyle.IsItalics &&
                this.IsUnderlined == passedStyle.IsUnderlined &&
                this.WrapText == passedStyle.WrapText;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Flag indicating whether the cell should be bold
        /// </summary>
        public bool IsBold
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether the cell should be italicized
        /// </summary>
        public bool IsItalics
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether text should wrap
        /// </summary>
        public bool WrapText
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether the cell should be underlined
        /// </summary>
        public bool IsUnderlined
        {
            get;
            set;
        }

        /// <summary>
        /// Numeric representation of font size
        /// </summary>
        public int FontSize
        {
            get
            {
                return _fontSize;
            }
            set
            {
                _fontSize = value;
                if (_fontSize > MAXIMUM_FONT_SIZE)
                    _fontSize = MAXIMUM_FONT_SIZE;
            }
        }

        /// <summary>
        /// String representation of Font face
        /// </summary>
        public CommonFonts FontFace
        {
            get;
            set;
        }

        /// <summary>
        /// Border Style
        /// </summary>
        public BorderTypes BorderType
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether there should be a top border
        /// </summary>
        public bool BorderTop
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether there should be a left border
        /// </summary>
        public bool BorderLeft
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether there should be a right border
        /// </summary>
        public bool BorderRight
        {
            get;
            set;
        }

        /// <summary>
        /// Flag indicating whether there should be a bottom border
        /// </summary>
        public bool BorderBottom
        {
            get;
            set;
        }

        /// <summary>
        /// Cell alignment
        /// </summary>
        public AlignStyles Alignment
        {
            get;
            set;
        }

        /// <summary>
        /// Cell background color
        /// </summary>
        public ExcelColors BackColor
        {
            get;
            set;
        }

        /// <summary>
        /// Cell Fore color
        /// </summary>
        public ExcelColors ForeColor
        {
            get;
            set;
        }

    }

}
