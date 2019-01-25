package com.TheDisappointedProgrammer.hello;

import org.apache.commons.csv.CSVRecord;
import org.springframework.stereotype.Component;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.stream.Stream;


@Component
public class IngredientTable extends McCanceBaseTable<Stream<McIngredient>> {
    @Override
    Stream<McIngredient> getData() throws IOException {
        return buildOutput(this.mcCanceReader);
    }

    private static Stream<McIngredient> buildOutput(McCanceReader mcCanceReader) throws IOException {
        List<McIngredient> list = new ArrayList<>();
        return
            mcCanceReader.getIngredientsParser().getRecords().stream().map(IngredientTable::map);
    }

    private static  McIngredient map(CSVRecord csvRecord) {
        return new McIngredient(
                csvRecord.get(0),
                csvRecord.get(1),
                csvRecord.get(2),
                csvRecord.get(3),
                csvRecord.get(4),
                csvRecord.get(5),
                csvRecord.get(6)
        );
    }
}
