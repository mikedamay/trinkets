package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVParser;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.util.*;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import java.util.stream.StreamSupport;

@Component
public class IngredientNutrientTable extends McCanceBaseTable<Stream<String[]>> {

    @Override
    Stream<String[]> getData() throws IOException {
//        return new ValuesAndFieldNames(mcCanceReader.getFieldNames(), mcCanceReader.getIngredientsParser());
        return makeIngredientNutrientRecords(mcCanceReader);
    }
    private static Stream<String[]> makeIngredientNutrientRecords(McCanceReader mcCanceReader) throws IOException {
        var s = mcCanceReader.getIngredientsParser().getRecords().stream();
        return s.map(s2 -> includeNutrientNames(s2, mcCanceReader.getFieldNames())).flatMap(
                r -> StreamSupport.stream(
                        Spliterators.spliteratorUnknownSize(
                                r.iterator(), Spliterator.ORDERED), false)
                        .map(ss -> new String[] { r.get(0)[1], ss[0], ss[1]}))
                .skip(McCanceReader.NUM_GENERAL_HEADINGS);
    }

    private static List<String[]> includeNutrientNames(Iterable<String> ingredientParts, String[] fieldNames) {
        List<String[]> list = new ArrayList<>();
        int ii = 0;
        for (var p : ingredientParts) {
            String[] nameAndValue = {fieldNames[ii], p};
            list.add(nameAndValue);
            ii++;
        }
        return list;
    }
    public class ValuesAndFieldNames {
        private String[] fieldNames;
        private CSVParser ingredientParser;

        public ValuesAndFieldNames(String[] fieldNames, CSVParser ingredientParser) {
            this.fieldNames = fieldNames;
            this.ingredientParser = ingredientParser;
        }

        public String[] getFieldNames() {
            return fieldNames;
        }

        public CSVParser getIngredientParser() {
            return ingredientParser;
        }

    }
}
