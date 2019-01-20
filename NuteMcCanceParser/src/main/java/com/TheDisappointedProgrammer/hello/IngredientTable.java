package com.TheDisappointedProgrammer.hello;

import org.springframework.stereotype.Component;

import java.util.ArrayList;
import java.util.List;



@Component
public class IngredientTable extends McCanceBaseTable<List<String[]>> {
    @Override
    List<String[]> getData() {
        return buildOutput(this.mcCanceReader);
    }

    private static List<String[]> buildOutput(McCanceReader mcCanceReader) {
        List<String[]> list = new ArrayList<>();
        for (var rec : mcCanceReader.getIngredientsParser()) {
            var output = new String[McCanceReader.NUM_GENERAL_HEADINGS];
            for (int ii = 0; ii < McCanceReader.NUM_GENERAL_HEADINGS; ii++) {
                output[ii] = rec.get(ii);
            }
            list.add(output);
        }
        return list;
    }
}
