package com.TheDisappointedProgrammer.hello;

import org.springframework.stereotype.Component;

import java.util.Map;
import java.util.stream.Stream;

@Component
public class NutrientTable extends McCanceBaseTable<Stream<String[]>> {
    @Override
    Stream<String[]> getData() {
        return mapToStream(mcCanceReader);
    }
    Stream<String[]> mapToStream(McCanceReader mcCanceReader) {
        return mcCanceReader.getNutrients().entrySet().stream().map(es -> new String[] {es.getKey(), es.getValue()});
    }
}
