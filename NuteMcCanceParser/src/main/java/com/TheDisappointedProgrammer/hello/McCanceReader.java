package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVFormat;
import org.apache.commons.csv.CSVParser;
import org.apache.commons.csv.CSVRecord;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.io.Reader;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@Component
public class McCanceReader {
    private final static int NUM_HEADERS = 3;
    public final static int NUM_GENERAL_HEADINGS = 7;
    private final static int GENERAL_HEADER_LINE = 0;
    private final static int ABBREVIATION_HEADER_LINE = 1;

    private final static String DATA_NOT_READY_MESSAGE = "McCanceReader.processMcCanceCSVFile() must be called before accessing data";

    private CSVParser ingredientsParser;
    private Map<String, String> nutrients;
    private String[] fieldNames;

    /**
     * @param rdr e.g. an InputStreamReader pointing at Mccance data
     * @return hint at support for a fluent API.
     * @throws IOException
     */
    public McCanceReader processMcCanceCSVFile(Reader rdr) throws IOException {
        CSVParser headerParser = CSVFormat.EXCEL.parse(rdr);
        List<CSVRecord> headerLines = getHeaderLines(headerParser);
        String[] headers = makeHeaders(headerLines);
        ingredientsParser = headerParser;
        nutrients = makeNutrientMap(headerLines);
        fieldNames = headers;
        return this;
    }

    private Map<String, String> makeNutrientMap(List<CSVRecord> headerLines) {
        assert (headerLines.size() == NUM_HEADERS);
        assert (headerLines.get(GENERAL_HEADER_LINE).size() > NUM_GENERAL_HEADINGS);
        assert (headerLines.get(GENERAL_HEADER_LINE).size() == headerLines.get(ABBREVIATION_HEADER_LINE).size());
        Map<String, String> map = new HashMap<>();
        for (int ii = NUM_GENERAL_HEADINGS; ii < headerLines.get(GENERAL_HEADER_LINE).size(); ii++) {
            map.put(headerLines.get(ABBREVIATION_HEADER_LINE).get(ii)
                    , headerLines.get(GENERAL_HEADER_LINE).get(ii));
        }
        return map;
    }

    /**
     * Can be called only after processMcCanceCSVFile()
     * @return  a csv paraer fully loaded with Mccance data except for the header info.
     */
    public CSVParser getIngredientsParser() {
        if (ingredientsParser == null)
            throw new RuntimeException(DATA_NOT_READY_MESSAGE);
        return ingredientsParser;
    }

    /**
     * Can be called only after processMcCanceCSVFile()
     * @return  a mapping of nutrient abbreviations to full names
     *      e.g. ALCO => Alcohol
     */
    public Map<String, String> getNutrients() {
        if (nutrients == null)
            throw new RuntimeException(DATA_NOT_READY_MESSAGE);
        return nutrients;
    }

    /**
     * Can be called only after processMcCanceCSVFile()
     * @return  an array of the field names relating to each ingredient record.
     *          The general field names are taken from header line 0 and the nutrient
     *          field names from header line 1
     */
    public String[] getFieldNames() {
        if (fieldNames == null)
            throw new RuntimeException(DATA_NOT_READY_MESSAGE);
        return fieldNames;
    }

    private String[] makeHeaders(List<CSVRecord> headerLines) {
        assert (headerLines.size() == NUM_HEADERS);
        String[] headers = new String[headerLines.get(0).size()];
        addGeneralHeaders(headerLines.get(GENERAL_HEADER_LINE), headers);
        addNutrientHeaders(headerLines.get(ABBREVIATION_HEADER_LINE), headers);
        return headers;
    }

    private void addGeneralHeaders(CSVRecord headerLine, String[] headers) {
        assert (headerLine.size() == headers.length);
        assert (headerLine.size() > NUM_GENERAL_HEADINGS);
        for (int ii = 0; ii < NUM_GENERAL_HEADINGS; ii++) {
            headers[ii] = headerLine.get(ii);
        }
    }

    private void addNutrientHeaders(CSVRecord headerLine, String[] headers) {
        assert (headerLine.size() == headers.length);
        assert (headerLine.size() > NUM_GENERAL_HEADINGS);
        for (int ii = NUM_GENERAL_HEADINGS; ii < headerLine.size(); ii++) {
            headers[ii] = headerLine.get(ii);
        }
    }

    private static List<CSVRecord> getHeaderLines(CSVParser parser) {
        var list = new ArrayList<CSVRecord>();
        int ctr = 0;
        for (CSVRecord rec : parser) {
            list.add(rec);
            if (++ctr == NUM_HEADERS)
                break;
        }
        if (ctr < NUM_HEADERS)
            throw new RuntimeException("McCance spreadsheet has too few header rows - should be: " + NUM_HEADERS);
        return list;
    }
}
