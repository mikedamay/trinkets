package com.TheDisappointedProgrammer.hello;

import org.springframework.stereotype.Component;

import java.util.Map;

@Component
public class NutrientTable extends McCanceBaseTable<Map<String, String>> {
    @Override
    Map<String, String> getData() {
        return mcCanceReader.getNutrients();
    }
}
