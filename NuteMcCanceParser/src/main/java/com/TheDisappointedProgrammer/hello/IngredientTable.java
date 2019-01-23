package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVRecord;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Spliterator;
import java.util.Spliterators;
import java.util.stream.Collectors;
import java.util.stream.Stream;
import java.util.stream.StreamSupport;


@Component
public class IngredientTable extends McCanceBaseTable<Stream<String[]>> {
    @Override
    Stream<String[]> getData() throws IOException {
        return buildOutput(this.mcCanceReader);
    }

    private static Stream<String[]> buildOutput(McCanceReader mcCanceReader) throws IOException {
        List<String[]> list = new ArrayList<>();
        return
            mcCanceReader.getIngredientsParser().getRecords().stream().map(IngredientTable::map);
    }

    private static  String[] map(CSVRecord strings) {
        return StreamSupport
                .stream(Spliterators.spliteratorUnknownSize(
                        strings.iterator(), Spliterator.ORDERED), false)
                .limit(McCanceReader.NUM_GENERAL_HEADINGS).toArray(String[]::new);
    }
}
