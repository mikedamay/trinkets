package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVRecord;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Spliterator;
import java.util.Spliterators;
import java.util.stream.Collectors;
import java.util.stream.StreamSupport;


@Component
public class IngredientTable extends McCanceBaseTable<List<String[]>> {
    @Override
    List<String[]> getData() throws IOException {
        return buildOutput(this.mcCanceReader);
    }

    private static List<String[]> buildOutput(McCanceReader mcCanceReader) throws IOException {
        List<String[]> list = new ArrayList<>();
        List<String[]> fs =
            mcCanceReader.getIngredientsParser().getRecords().stream().map(IngredientTable::map).collect(Collectors.toList());
 /*       for (var rec : mcCanceReader.getIngredientsParser()) {
            var output = new String[McCanceReader.NUM_GENERAL_HEADINGS];
            for (int ii = 0; ii < McCanceReader.NUM_GENERAL_HEADINGS; ii++) {
                output[ii] = rec.get(ii);
            }
            list.add(output);
        }
*/        return fs;
    }

    private static  String[] map(CSVRecord strings) {
        return StreamSupport
                .stream(Spliterators.spliteratorUnknownSize(
                        strings.iterator(), Spliterator.ORDERED), false)
                .limit(McCanceReader.NUM_GENERAL_HEADINGS).toArray(String[]::new);
    }
}
