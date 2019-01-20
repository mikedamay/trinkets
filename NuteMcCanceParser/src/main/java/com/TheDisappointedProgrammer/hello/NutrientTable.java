package com.TheDisappointedProgrammer.hello;

import org.springframework.context.annotation.Scope;
import org.springframework.stereotype.Component;

import java.util.Map;

@Component
public class NutrientTable extends McCanceTable<Map<String, String>> {
    @Override
    Map<String, String> getData() {
        return mcCanceReader.getNutrients();
    }
}
